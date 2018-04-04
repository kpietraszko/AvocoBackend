using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AvocoBackend.Data.Models;
using Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _config;
		private readonly IPasswordHasher<User> _passwordHasher;

		public UserController(ApplicationDbContext context, IConfiguration config, IPasswordHasher<User> passwordHasher)
		{
			_context = context;
			_config = config;
			_passwordHasher = passwordHasher;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterModel userData) //w js body requestu powinno byc new FormData(form), gdzie form to <form/> htmlowy
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (_context.Users.Any(u => u.EmailAddress == userData.EmailAddress))
			{
				return StatusCode(422, "Email already used");

			}
			var hashedPassword = _passwordHasher.HashPassword(null, userData.Password);
			var newUser = new User{
				EmailAddress = userData.EmailAddress,
				PasswordHash = hashedPassword,
				FirstName = userData.FirstName,
				LastName = userData.LastName,
				Region = userData.Region
			};
			_context.Users.Add(newUser);
			await _context.SaveChangesAsync();

			return CreatedAtAction("Register", null);
		}
		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login([FromBody]LoginModel loginModel) //zwraca token
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			IActionResult response = Unauthorized();
			var user = Authenticate(loginModel);
			if (user != null)
			{
				var tokenString = BuildToken(user);
				response = Ok(new { token = tokenString });
			}
			return response;
		}
		User Authenticate(LoginModel loginModel)
		{
			User user = null;
			var emailMatchingUser = _context.Users.FirstOrDefault(u => u.EmailAddress == loginModel.Email);
			if (emailMatchingUser != null)
			{
				var hashVerificationResult = _passwordHasher.VerifyHashedPassword(null, emailMatchingUser.PasswordHash, loginModel.Password);
				if (hashVerificationResult == PasswordVerificationResult.Success)
					user = emailMatchingUser;
			}
			return user;
		}
		string BuildToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Issuer"],
				expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:TTL", 30)), //czas z configu, jesli w configu brak to 30min
				signingCredentials: creds
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}