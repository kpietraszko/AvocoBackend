using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class CreateGroupDTO
	{
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public IFormFile GroupImage { get; set; }
	}
}
