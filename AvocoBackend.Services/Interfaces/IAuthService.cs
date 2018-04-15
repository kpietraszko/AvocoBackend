using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;

namespace AvocoBackend.Services.Interfaces
{
	public interface IAuthService
	{
		ServiceResult<User> Register(RegisterDTO model);
		ServiceResult<TokenDTO> Login(LoginDTO model);
	}
}
