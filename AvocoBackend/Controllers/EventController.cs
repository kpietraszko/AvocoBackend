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
		[HttpPost("/api/[controller]/[action]/{groupId}")]
		public IActionResult Create([FromForm]EventDTO newEvent, int groupId)
		{
			if (!ModelState.IsValid)
			{
				return StatusCode(422, ModelState);
			}
			var result = _eventService.CreateEvent(newEvent, groupId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{eventId:int}")]
		public IActionResult GetDetails(int eventId)
		{
			var result = _eventService.GetDetails(eventId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{eventId:int}/interestedUsers")]
		public IActionResult GetInterestedUsers(int eventId)
		{
			var result = _eventService.GetInterestedUsers(eventId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{eventId:int}/groupImage")]
		public IActionResult GetGroupImage(int eventId)
		{
			var result = _eventService.GetGroupImage(eventId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpPut("/api/[controller]/{eventId:int}/interested/{interested:bool}")]
		public IActionResult SetInterested(int eventId, bool interested)
		{
			var result = _eventService.SetInterested(eventId, interested, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
	}
}