using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class RegisterModel
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string Region { get; set; }
		[Required]
		[EmailAddress]
		public string EmailAddress { get; set; }
		[Required]
		[MinLength(6)]
		public string Password { get; set; }
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}
