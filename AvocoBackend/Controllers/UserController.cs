using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using AvocoBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[Authorize]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _dbContext;

		public UserController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpPost]
		public async Task<IActionResult> Photo(IFormFile file)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var reqUserIdString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
			int reqUserId;
			if (Int32.TryParse(reqUserIdString, out reqUserId))
			{
				using (var memoryStream = new MemoryStream())
				{
					if (file?.Length > 0)
					{
						await file.CopyToAsync(memoryStream);
						var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == reqUserId);
						if (dbUser != null)
						{
							dbUser.ProfileImage = memoryStream.ToArray();
							response = Ok();
						}
					}
				}
				await _dbContext.SaveChangesAsync();
			}

			return response;
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Photo(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
			if (user?.ProfileImage != null)
			{
				response = File(user.ProfileImage, "image/png");
			}
			return response;
		}
		[HttpPost]
		public async Task<IActionResult> UserInfo(string firstName, string lastName, string region)
		{
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
			if (dbUser != null)
			{
				dbUser.FirstName = firstName ?? dbUser.FirstName;
				dbUser.LastName = lastName ?? dbUser.LastName;
				dbUser.Region = region ?? dbUser.Region;
				await _dbContext.SaveChangesAsync();
			}

			return response;
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult UserInfo(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
			if (dbUser != null)
			{
				response = Json(new {
					fullName = $"{dbUser.FirstName} {dbUser.LastName}",
					region = dbUser.Region
				});
			}
			return response;
		}
		[HttpGet("{searchText}")]
		public IActionResult SearchInterests(string searchText)
		{
			if (String.IsNullOrWhiteSpace(searchText))
				return StatusCode(422);
			var found =  _dbContext.Interests.Where(i => i.InterestName.Contains(searchText));
			return Ok(found);
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Interests(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var userInterests = _dbContext.UsersInterests.Include(ui => ui.Interest).Include(ui => ui.User).Where(ui => ui.UserId == userId);
			var interests = userInterests.Select(ui => ui.Interest.InterestName);
			return Json(interests);
		}
		[HttpPost("{interestId:int?}")]
		[HttpPost("{interestName?}")]
		public async Task<IActionResult> AddInterest(int? interestId = null, string interestName = null)
		{
			if (interestId == null && interestName == null)
				return BadRequest();
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
			if (dbUser != null)
			{
				if (interestId != null) //dodaj istniejace zainteresowanie uzytkownikowi
				{
					if (_dbContext.Interests.FirstOrDefault(i => i.InterestId == interestId) != null && //jesli zainteresowanie istnieje 
						_dbContext.UsersInterests.FirstOrDefault(ui => ui.UserId == userId && ui.InterestId == interestId) == null) //i ten uzytkownik go nie ma
						_dbContext.UsersInterests.Add(new UserInterest { UserId = (int)userId, InterestId = (int)interestId });
					response = Ok();
				}
				else //stworz zainteresowanie
				{
					if (_dbContext.Interests.FirstOrDefault(i => i.InterestName == interestName) == null) //jesli zainteresowanie o tej nazwie nie istnieje
					{
						var newInterest = _dbContext.Interests.Add(new Interest { InterestName = interestName });
						_dbContext.UsersInterests.Add(new UserInterest { UserId = (int)userId, InterestId = newInterest.Entity.InterestId });
						response = Ok();
					}

				}
				await _dbContext.SaveChangesAsync();
			}
			return response;
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Groups(int userId)
		{
			IActionResult response = StatusCode(422);
			if (_dbContext.Users.FirstOrDefault(u => u.UserId == userId) != null)
			{
				var groups = _dbContext.GroupsJoinedUsers.Where(g => g.UserId == userId);
				var groupsData = groups.Include(g => g.Group).Select(g => new { groupId = g.GroupId, groupName = g.Group.GroupName });
				response = Json(groupsData);
			}
			return response;
		}
		[HttpGet()]
		public IActionResult Friends()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			var friends = _dbContext.Friends.Where(f => f.User1Id == userId || f.User2Id == userId);
			var friendsData = friends.Include(f => f.User1).Include(f => f.User2)
				.Select(f =>
					f.User1Id == userId ? (new { userId = f.User2Id, fullName = $"{f.User2.FirstName} {f.User2.LastName}"}) :
					(new { userId = f.User1Id, fullName = $"{f.User1.FirstName} {f.User1.LastName}"})
			);
			return Json(friendsData);
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public async Task<IActionResult> AddFriend(int user2Id)
		{
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			if (userId == user2Id)
				return response;
			var alreadyExists = _dbContext.Friends.FirstOrDefault(f => (f.User1Id == userId && f.User2Id == user2Id) ||
			(f.User2Id == userId && f.User1Id == user2Id)) != null;
			if (!alreadyExists)
			{
				if (_dbContext.Users.FirstOrDefault(u => u.UserId == userId) != null &&
					_dbContext.Users.FirstOrDefault(u => u.UserId == user2Id) != null)
					_dbContext.Friends.Add(new Friend { User1Id = (int)userId, User2Id = user2Id });
				await _dbContext.SaveChangesAsync();
				response = Ok();
			}
			return response;
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public async Task<IActionResult> Unfriend(int user2Id)
		{
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			if (user2Id == userId)
				return response;
			var user2exists = _dbContext.Users.FirstOrDefault(u => u.UserId == user2Id) != null;
			if(user2exists)
			{
				var friendship = _dbContext.Friends.FirstOrDefault(f => (f.User1Id == userId && f.User2Id == user2Id) ||
					(f.User1Id == user2Id && f.User2Id == userId));
				if (friendship != null)
				{
					_dbContext.Friends.Remove(friendship);
					await _dbContext.SaveChangesAsync();
					response = Ok();
				}
			}
			return response;
		}
		private int? GetUserIdFromClaims(HttpContext context)
		{
			var reqUserIdString = context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
			int reqUserId;
			if (Int32.TryParse(reqUserIdString, out reqUserId))
			{
				return reqUserId;
			}
			return null;
		}
	}
}