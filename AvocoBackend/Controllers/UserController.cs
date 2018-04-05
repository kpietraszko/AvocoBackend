using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

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
			Console.WriteLine("Claims of user posting this Photo request");
			Console.WriteLine(HttpContext.User.Claims);
			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				//TODO: dowiedziec się kto wysyła ten request, znalezc tego uzytkownika w bazie i zapisac mu ten obrazek
			}

			return Ok();
		}
		[HttpGet]
		public async Task<IActionResult> Photo(int userId)
		{
			await Task.Delay(1);
			return null;
		}

	 }
}