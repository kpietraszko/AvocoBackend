using Microsoft.AspNetCore.Mvc;
using AvocoBackend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using AvocoBackend.Services.Interfaces;
using AvocoBackend.Data.DTOs;

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IConfiguration _config;
		private readonly IPasswordHasher<User> _passwordHasher;

		public AuthenticationController(IAuthService authService, IConfiguration config, IPasswordHasher<User> passwordHasher)
		{
			_authService = authService;
			_config = config;
			_passwordHasher = passwordHasher;
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Register([FromBody] RegisterDTO userData)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = _authService.Register(userData);
			if (result.IsError)
				return StatusCode(422, result.Errors);

			return Ok(result.SuccessResult);
		}
		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login([FromBody]LoginDTO loginData) //zwraca token
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = _authService.Login(loginData);
			if (result.IsError)
				return Unauthorized();

			return Ok(result.SuccessResult);
		}
	}
}