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
		ServiceResult<byte[]> GetImage(string path);
		ServiceResult<ImagePathsDTO> SaveImages(int id, IFormFile sentFile, string directory);
	}
}
