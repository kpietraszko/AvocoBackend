﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
    public class UserInfo
    {
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? Region { get; set; } 
		//jeszcze ProfileImage
	}
}
