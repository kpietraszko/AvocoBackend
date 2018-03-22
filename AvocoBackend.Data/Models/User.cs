using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string EmailAddress { get; set; }
		//hash hasla raczej w oddzielnej tabeli
		public string PasswordHash { get; set; } 
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Region { get; set; }
	}
}
