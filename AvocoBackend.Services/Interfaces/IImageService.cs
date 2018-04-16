using AvocoBackend.Services.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AvocoBackend.Data.DTOs;

namespace AvocoBackend.Services.Interfaces
{
    public interface IImageService
    {
		ServiceResult<byte[]> GetUserImage(string path);
		ServiceResult<ImagePathsDTO> SaveUserImages(int userId, IFormFile sentFile);
	}
}
