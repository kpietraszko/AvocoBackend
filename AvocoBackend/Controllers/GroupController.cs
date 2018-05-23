using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

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
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult GroupInterests(int groupId)
		{
			var result = _groupService.GetGroupInterests(groupId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult Image(int groupId)
		{
			var result = _groupService.GetImage(groupId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return File(result.SuccessResult, "image/png");
		}
		[HttpPost("/api/[controller]/{groupId}/[action]")]
		public IActionResult AddPost(int groupId, [FromForm]string postContent) //frombody nie działa
		{

			var result = _groupService.AddPost(groupId, postContent, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult Posts(int groupId)
		{
			var result = _groupService.GetGroupsPosts(groupId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpPost("/api/[controller]/[action]/{postId}")]
		public IActionResult AddComment(int postId, [FromForm]string comment)
		{
			var result = _groupService.AddComment(postId, comment, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpPut("/api/[controller]/{groupId}/[action]")]
		public IActionResult JoinGroup(int groupId)
		{
			var result = _groupService.JoinGroup(groupId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpPut("/api/[controller]/{groupId}/[action]")]
		public IActionResult LeaveGroup(int groupId)
		{
			var result = _groupService.LeaveGroup(groupId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult UserInGroup(int groupId)
		{
			var result = _groupService.UserInGroup(groupId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")]
		public IActionResult Events(int groupId)
		{
			var result = _groupService.GetEvents(groupId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpDelete("/api/[controller]/[action]/{postId}")]
		public IActionResult DeletePost(int postId)
		{
			var result = _groupService.DeletePost(postId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpDelete("/api/[controller]/[action]/{commentId}")]
		public IActionResult DeleteComment(int commentId)
		{
			var result = _groupService.DeleteComment(commentId, HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet]
		public IActionResult AllGroups()
		{
			var result = _groupService.GetAllGroups();
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}

	}
}