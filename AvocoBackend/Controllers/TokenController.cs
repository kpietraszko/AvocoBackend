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

namespace AvocoBackend.Api.Controllers
{
	[Route("api/[controller]")]
	public class TokenController : Controller
	{
		private IConfiguration _config;

		public TokenController(IConfiguration config) //DI dziala
		{
			_config = config;
		}
		
		[AllowAnonymous]
		[HttpPost]
		public IActionResult CreateToken([FromBody]LoginModel loginModel) //loginModel przychodzi ok
		{
			IActionResult response = Unauthorized();
			var user = Authenticate(loginModel); 
			if(user != null)
			{
				var tokenString = BuildToken(user); //TODO: zaimplementowac
				response = Ok(new { token = tokenString });
			}
			return response;
		}
		User Authenticate(LoginModel loginModel)
		{
			User user = null;
			//TODO: spr email z bazą, zahashuj haslo i porownac z hashem z bazy
			if (loginModel.Email == "user@example.com" && loginModel.Password == "secretPassword") //HACK
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