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

namespace AvocoBackend.Api.Controllers
{
	[Route("api/register")]
	public class RegisterController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IPasswordHasher<User> _passwordHasher;

		public RegisterController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}

		// POST: api/Register
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromForm] RegisterModel userData) //w js body requestu powinno byc new FormData(form), gdzie form to <form/> htmlowy
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if(_context.Users.Any(u => u.EmailAddress == userData.EmailAddress))
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

			return CreatedAtAction("CreateUser", null);
		}
	}
}