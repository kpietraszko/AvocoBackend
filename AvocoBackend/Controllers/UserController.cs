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
	//[Authorize]
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
		[HttpGet("{userId}")]
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
			var reqUserIdString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
			int reqUserId;
			if (Int32.TryParse(reqUserIdString, out reqUserId))
			{
				var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == reqUserId);
				if (dbUser != null)
				{
					dbUser.FirstName = firstName ?? dbUser.FirstName;
					dbUser.LastName = lastName ?? dbUser.LastName;
					dbUser.Region = region ?? dbUser.Region;
					await _dbContext.SaveChangesAsync();
				}
			}
			return response;
		}
		[HttpGet("{userId}")]
		public IActionResult Name(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var dbUser = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
			if (dbUser != null)
			{
				response = Json(new { fullName = $"{dbUser.FirstName} {dbUser.LastName}" });
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
		[HttpGet("{userId}")]
		public IActionResult GetUsersInterests(int userId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = StatusCode(422);
			var usersInterestsTable = _dbContext.UsersInterests;
			var userInterests = _dbContext.UsersInterests.Include(ui => ui.Interest).Include(ui => ui.User).Where(ui => ui.UserId == userId);
			var interests = userInterests.Select(ui => ui.Interest.InterestName);
			return Json(interests);
		}


	}
}