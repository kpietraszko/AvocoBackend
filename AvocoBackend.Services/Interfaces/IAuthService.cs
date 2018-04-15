using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvocoBackend.Services.Interfaces
{
	public interface IAuthService
	{
		ServiceResult<User> Register(RegisterDTO model);
		ServiceResult<TokenDTO> Login(LoginDTO model);
	}
}
