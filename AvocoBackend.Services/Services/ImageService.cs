using AvocoBackend.Data.DTOs;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public class ImageService : IImageService
	{
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _hostingEnvironment;

		public ImageService(IConfiguration config, IHostingEnvironment hostingEnvironment)
		{
			_config = config;
			_hostingEnvironment = hostingEnvironment;
		}

		public ServiceResult<byte[]> GetImage(string path)
		{
			var absolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, path);
			try
			{
				using (var stream = new FileStream(absolutePath, FileMode.Open))
				{
					using (var ms = new MemoryStream())
					{
						stream.CopyTo(ms);
						return new ServiceResult<byte[]>(ms.ToArray());
					}
				}
			}
			catch (Exception e)
			{
				return new ServiceResult<byte[]>(e.Message);
			}
		}

		public ServiceResult<ImagePathsDTO> SaveImages(int id, IFormFile sentFile, string directory)
		{
			var contentRoot = _hostingEnvironment.ContentRootPath;
			string pathFromConfig = _config.GetValue<string>($"Images:Directories:{directory}");
			var orgImageName = id.ToString() + ".png";
			var smallImageName = id.ToString() + "_small" + ".png";
			var originalImageRelPath = Path.Combine(pathFromConfig, orgImageName);
			var originalImagePath = Path.Combine(contentRoot, originalImageRelPath);
			var smallImageRelPath = Path.Combine(pathFromConfig, smallImageName);
			var smallImagePath = Path.Combine(contentRoot, smallImageRelPath);
			int smallMaxSize = _config.GetValue<int>("Images:Sizes:Small");

			try
			{
				using (var image = Image.Load(sentFile.OpenReadStream()))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(originalImagePath));
					using (var fileStream = new FileStream(originalImagePath, FileMode.Create))
					{
						image.SaveAsPng(fileStream);
					}
					int biggerDim = Math.Max(image.Width, image.Height);
					if (biggerDim > smallMaxSize)
					{
						int finalWidth = image.Width > image.Height ? (smallMaxSize)
						: (int)(image.Width * ((double)smallMaxSize/image.Height) );
						int finalHeight = image.Height > image.Width ? (smallMaxSize)
						: (int)(image.Height * ((double)smallMaxSize/image.Width) );
						image.Mutate(x => x.Resize(finalWidth, finalHeight));
						using (var fileStream = new FileStream(smallImagePath, FileMode.Create))
						{
							image.SaveAsPng(fileStream);
						}
					}
				}
				return new ServiceResult<ImagePathsDTO>(new ImagePathsDTO
				{
					ImagePath = originalImageRelPath,
					ImageSmallPath = smallImageRelPath
				});
			}
			catch (Exception e)
			{
				return new ServiceResult<ImagePathsDTO>(e.Message);
			}

		}
	}
}
