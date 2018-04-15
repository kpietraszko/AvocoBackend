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

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[Authorize]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _dbContext; //repo zamiast
		private readonly IMapper _mapper;

		public UserController(ApplicationDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}
		[HttpPut]
		public async Task<IActionResult> Photo(IFormFile file)
		{
			IActionResult response = StatusCode(422);
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();

			using (var memoryStream = new MemoryStream())
			{
				if (file?.Length > 0)
				{
					await file.CopyToAsync(memoryStream);
					var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
					if (dbUser != null)
					{
						dbUser.ProfileImage = memoryStream.ToArray();
						response = Ok(); //od razu zwracac
					}
				}
			}
			await _dbContext.SaveChangesAsync();

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
			var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
			if (user?.ProfileImage != null)
			{
				response = File(user.ProfileImage, "image/png");
			}
			return response;
		}
		[HttpPut]
		public async Task<IActionResult> UserInfo(UserDTO userInfo) //string firstName, string lastName, int? region) //to chyba powinien być model
		{
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
			if (dbUser != null)
			{ //automapper
				var mapped = _mapper.Map(userInfo, dbUser);
				//dbUser.FirstName = firstName ?? dbUser.FirstName;
				//dbUser.LastName = lastName ?? dbUser.LastName;
				//dbUser.Region = region ?? dbUser.Region;
				await _dbContext.SaveChangesAsync();
				return Ok();
			}

			return StatusCode(422);
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult UserInfo(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
			if (dbUser != null)
			{
				var userInfo = _mapper.Map<UserDTO>(dbUser);
				return Ok(userInfo);
			}
			return StatusCode(422);
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
			if(_dbContext.Users.FirstOrDefault(u => u.Id == userId) == null)
				return StatusCode(422, "User doesn't exist");
			var userInterests = _dbContext.UsersInterests.Include(ui => ui.Interest).Include(ui => ui.User)
					 .Where(ui => ui.UserId == userId);
			var interests = userInterests.Include(ui => ui.Interest)
				.Select(ui => new { interestId = ui.InterestId, interestName = ui.Interest.InterestName} );
			return Ok(interests);
		}
		[HttpPost("{interestId:int?}")]
		[HttpPost("{interestName?}")]
		public async Task<IActionResult> AddInterest(int? interestId = null, string interestName = null)
		{
			if (interestId == null && interestName == null)
			{
				return BadRequest();
			}
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
			if (dbUser != null)
			{
				if (interestId != null) //dodaj istniejace zainteresowanie uzytkownikowi
				{
					if (_dbContext.Interests.FirstOrDefault(i => i.Id == interestId) != null && //jesli zainteresowanie istnieje 
						_dbContext.UsersInterests.FirstOrDefault(ui => ui.UserId == userId && ui.InterestId == interestId) == null) //i ten uzytkownik go nie ma
						_dbContext.UsersInterests.Add(new UserInterest { UserId = (int)userId, InterestId = (int)interestId });
				}
				else //stworz zainteresowanie
				{
					if (_dbContext.Interests.FirstOrDefault(i => i.InterestName == interestName) == null) //jesli zainteresowanie o tej nazwie nie istnieje
					{
						var newInterest = _dbContext.Interests.Add(new Interest { InterestName = interestName });
						_dbContext.UsersInterests.Add(new UserInterest { UserId = (int)userId, InterestId = newInterest.Entity.Id });
					}

				}
				await _dbContext.SaveChangesAsync();
				return Ok();
			}
			return StatusCode(422, "User doesn't exist");
		}
		[HttpGet("/api/[controller]/{userId}/[action]")]
		public IActionResult Groups(int userId)
		{
			if (_dbContext.Users.FirstOrDefault(u => u.Id == userId) != null)
			{
				var groups = _dbContext.GroupsJoinedUsers.Where(g => g.UserId == userId);
				var groupsData = groups.Include(g => g.Group)
					.Select(g => new
					{
						groupId = g.GroupId,
						groupName = g.Group.GroupName
					});
				return Ok(groupsData);
			}
			return StatusCode(422, "User doesn't exist");
		}
		[HttpGet("/api/[controller]/{groupId}/[action]")] //TODO: przeniesc do kontrolera grupy
		public IActionResult GroupPicture(int groupId)
		{
			var groupDb = _dbContext.Groups.FirstOrDefault(g => g.Id == groupId);
			if (groupDb != null)
				if (groupDb.GroupPicture != null)
					return File(groupDb.GroupPicture, "image/png");
			return StatusCode(422, "User doesn't exist");
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
			return Ok(friendsData);
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public async Task<IActionResult> AddFriend(int user2Id)
		{
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			if (userId == user2Id)
				return StatusCode(422, "User1 == User2");
			var alreadyExists = _dbContext.Friends.FirstOrDefault(f => (f.User1Id == userId && f.User2Id == user2Id) ||
			(f.User2Id == userId && f.User1Id == user2Id)) != null;
			if (!alreadyExists)
			{
				if (_dbContext.Users.FirstOrDefault(u => u.Id == userId) != null &&
					_dbContext.Users.FirstOrDefault(u => u.Id == user2Id) != null)
					_dbContext.Friends.Add(new Friend { User1Id = (int)userId, User2Id = user2Id });
				await _dbContext.SaveChangesAsync();
				return Ok();
			}
			return StatusCode(422, "Friendship already exists");
		}
		[HttpPut("/api/[controller]/{user2Id}/[action]")]
		public async Task<IActionResult> Unfriend(int user2Id)
		{
			var userId = GetUserIdFromClaims(HttpContext);
			if (userId == null)
				return Unauthorized();
			if (user2Id == userId)
				return StatusCode(422, "User1 == User2");
			var user2exists = _dbContext.Users.FirstOrDefault(u => u.Id == user2Id) != null;
			if (user2exists)
			{
				var friendship = _dbContext.Friends.FirstOrDefault(f => (f.User1Id == userId && f.User2Id == user2Id) || //tylko czy Friend istnieje, nie user
					(f.User1Id == user2Id && f.User2Id == userId));
				if (friendship != null)
				{
					_dbContext.Friends.Remove(friendship);
					await _dbContext.SaveChangesAsync();
					return Ok();
				}
			}
			return StatusCode(422, "User2 doesn't exist");
		}
		private int? GetUserIdFromClaims(HttpContext context)
		{
			var reqUserIdString = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
			if (Int32.TryParse(reqUserIdString, out int reqUserId))
			{
				return reqUserId;
			}
			return null;
		}
	}
}