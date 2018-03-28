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

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]")]
	public class TokenController : Controller
	{
		private IConfiguration _config; //dziala z DI
		private IPasswordHasher<User> _passwordHasher;

		public TokenController(IConfiguration config, IPasswordHasher<User> passwordHasher)
		{
			_config = config;
			_passwordHasher = passwordHasher;
		}
		
		[AllowAnonymous]
		[HttpPost]
		public IActionResult CreateToken([FromBody]LoginModel loginModel) //loginModel przychodzi ok
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
			if (loginModel.Email == "user@example.com" &&
				_passwordHasher.VerifyHashedPassword(
					null, _passwordHasher.HashPassword(null, "secretPassword") , loginModel.Password) == PasswordVerificationResult.Success)  //HACK
			{
				user = new User(loginModel.Email);
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
				expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:TTL", 30)),
				signingCredentials: creds
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}