using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AvocoBackend.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Repository;

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]")]
	public class TokenController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _config; //dziala z DI
		private readonly IPasswordHasher<User> _passwordHasher;

		public TokenController(ApplicationDbContext context, IConfiguration config, IPasswordHasher<User> passwordHasher)
		{
			_context = context;
			_config = config;
			_passwordHasher = passwordHasher;
		}
		
		[AllowAnonymous]
		[HttpPost]
		public IActionResult CreateToken([FromForm]LoginModel loginModel) //loginModel przychodzi ok
		{
			IActionResult response = Unauthorized();
			var user = Authenticate(loginModel); 
			if(user != null)
			{
				var tokenString = BuildToken(user);
				response = Ok(new { token = tokenString });
			}
			return response;
		}
		User Authenticate(LoginModel loginModel)
		{
			User user = null;
			//TODO: spr email z bazą, zahashuj haslo i porownac z hashem z bazy
			var emailMatchingUser = _context.Users.FirstOrDefault(u => u.EmailAddress == loginModel.Email);
			if(emailMatchingUser != null)
			{
				if (_passwordHasher.VerifyHashedPassword(null, emailMatchingUser.PasswordHash, loginModel.Password) == PasswordVerificationResult.Success)
					user = emailMatchingUser;
			}

			//if (loginModel.Email == "user@example.com" &&
			//	_passwordHasher.VerifyHashedPassword(
			//		null, _passwordHasher.HashPassword(null, "secretPassword") , loginModel.Password) == PasswordVerificationResult.Success)  //HACK
			//{
			//	user = new User(loginModel.Email); //HACK
			//}
			return user;
		}
		string BuildToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Issuer"],
				expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:TTL", 30)),
				signingCredentials: creds
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}