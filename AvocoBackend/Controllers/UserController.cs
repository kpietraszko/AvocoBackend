using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AvocoBackend.Repository;
using AvocoBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly IClaimsService _claimsService;

		public UserController(IUserService userService, IClaimsService claimsService)
		{
			_userService = userService;
			_claimsService = claimsService;
		}
		//[HttpPut]
		//public async Task<IActionResult> Photo(IFormFile file)
		//{
		//	IActionResult response = StatusCode(422);
		//	var userId = GetUserIdFromClaims(HttpContext);
		//	if (userId == null)
		//		return Unauthorized();

		//	using (var memoryStream = new MemoryStream())
		//	{
		//		if (file?.Length > 0)
		//		{
		//			await file.CopyToAsync(memoryStream);
		//			var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		//			if (dbUser != null)
		//			{
		//				dbUser.ProfileImage = memoryStream.ToArray();
		//				response = Ok(); //od razu zwracac
		//			}
		//		}
		//	}
		//	await _dbContext.SaveChangesAsync();

		//	return response;
		//}
		//[HttpGet("/api/[controller]/{userId}/[action]")]
		//public IActionResult Photo(int userId)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}
		//	IActionResult response = StatusCode(422);
		//	var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		//	if (user?.ProfileImage != null)
		//	{
		//		response = File(user.ProfileImage, "image/png");
		//	}
		//	return response;
		//}
		[HttpPut]
		public IActionResult UserInfo(UserDTO userInfo) //string firstName, string lastName, int? region) //to chyba powinien być model
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = _userService.SetUserInfo(userInfo, HttpContext);
			return result.IsError ? StatusCode(422, result.Errors) :
				Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult UserInfo(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = _userService.GetUserInfo(userId);
			return result.IsError ? StatusCode(422, result.Errors) :
				Ok(result.SuccessResult);
		}
		[HttpGet("{searchText}")]
		public IActionResult SearchInterests(string searchText)
		{
			var result = _userService.SearchInterests(searchText);
			return result.IsError ? StatusCode(422, result.Errors) :
				Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Interests(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = _userService.GetInterests(userId);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpPost("{interestId:int?}")]
		[HttpPost("{interestName?}")]
		public IActionResult AddInterest(int? interestId = null, string interestName = null)
		{
			if (interestId == null && interestName == null)
			{
				return BadRequest();
			}
			var result = _userService.AddInterest(HttpContext, interestId, interestName);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Groups(int userId)
		{
			var result = _userService.GetGroups(userId);
			return result.IsError ? StatusCode(422, result.Errors) : Ok(result.SuccessResult);
		}

		[HttpGet()]
		public IActionResult Friends()
		{
			var result = _userService.GetFriends(HttpContext);
			return result.IsError ? StatusCode(422, result.Errors) : Ok(result.SuccessResult);
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public IActionResult AddFriend(int user2Id)
		{
			var result = _userService.AddFriend(user2Id, HttpContext);
			return result.IsError ? StatusCode(422, result.Errors) : Ok(result.SuccessResult);
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public IActionResult Unfriend(int user2Id)
		{
			var result = _userService.Unfriend(user2Id, HttpContext);
			return result.IsError ? StatusCode(422, result.Errors) : Ok(result.SuccessResult);
		}
	}
}