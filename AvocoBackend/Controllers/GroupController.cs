using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvocoBackend.Api.Controllers
{
	[Authorize]
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
			if (!ModelState.IsValid)
			{
				return StatusCode(422, ModelState);
			}
			var result = _groupService.CreateGroup(groupData);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult GroupInfo(int groupId)
		{
			var result = _groupService.GetGroupInfo(groupId);
			if (result.IsError)
			{
				return StatusCode(422, ModelState);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult GroupInterests(int groupId)
		{
			var result = _groupService.GetGroupInterests(groupId);
			if (result.IsError)
			{
				return StatusCode(422, ModelState);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult Image(int groupId)
		{
			var result = _groupService.GetImage(groupId);
			if (result.IsError)
			{
				return StatusCode(422, ModelState);
			}
			return File(result.SuccessResult, "image/png");
		}
	}
}