using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public class ClaimsService : IClaimsService
	{
		private readonly IRepository<User> _userRepository;

		public ClaimsService(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public T GetFromClaims<T>(HttpContext httpContext, string claimType)
		{
			string result = httpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
			try
			{
				return (T)Convert.ChangeType(result, Nullable.GetUnderlyingType(typeof(T)));
			}
			catch
			{
				return default(T);
			}

		}
	}
}
