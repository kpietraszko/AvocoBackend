using AvocoBackend.Data.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Interfaces
{
    public interface IHomeService
    {
		ServiceResult<PostDTO[]> GetFeedPosts(HttpContext httpContext);
	}
}
