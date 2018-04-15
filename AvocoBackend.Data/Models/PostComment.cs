using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class PostComment :BaseModel
	{
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		[Required]
		public string Content { get; set; }
	}
}
