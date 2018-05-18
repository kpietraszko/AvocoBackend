using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvocoBackend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
	 [Authorize]
    public class HomeController : Controller
    {
		IHomeService _homeService;
		public HomeController(IHomeService homeService)
		{
			_homeService = homeService;
		}
		[HttpGet]
		public IActionResult Feed()
		{
			var result = _homeService.GetFeedPosts(HttpContext);
			if (result.IsError)
			{
				return StatusCode(422, result.Errors);
			}
			return Ok(result.SuccessResult);
		}
    }
}