using Microsoft.AspNetCore.Http;

namespace AvocoBackend.Services.Interfaces
{
    public interface IClaimsService
    {
		T GetFromClaims<T>(HttpContext httpContext, string claimType);
	}
}
