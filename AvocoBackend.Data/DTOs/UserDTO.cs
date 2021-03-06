﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
    public class UserDTO
    {
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? Region { get; set; }
		public byte[] Image { get; set; }
		[JsonIgnore]
		public string ImageSmallPath { get; set; }
	}
}
