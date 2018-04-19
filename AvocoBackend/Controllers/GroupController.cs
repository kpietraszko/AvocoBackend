using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvocoBackend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GroupController : Controller
    {
		IGroupService _groupService;
		public GroupController(IGroupService groupService)
		{
			_groupService = groupService;
		}

		[HttpPost]
		public IActionResult Create([FromForm]CreateGroupDTO groupData) //FromBody nie działa z jakiegos powodu
		{
			if(!ModelState.IsValid)
			{
				return StatusCode(422, ModelState);
			}
			var result = _groupService.CreateGroup(groupData);
			if(result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
    }
}