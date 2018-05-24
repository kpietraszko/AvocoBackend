using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvocoBackend.Api.Controllers
{
	[Authorize]
	[Route("api/Event")]
	public class EventController : Controller
	{
		private readonly IEventService _eventService;

		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}
		[HttpPost]
		public IActionResult Create(EventDTO newEvent)
		{
			if (!ModelState.IsValid)
			{
				return StatusCode(422, ModelState);
			}
			var result = _eventService.CreateEvent(newEvent);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
	}
}