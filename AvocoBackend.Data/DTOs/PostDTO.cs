using AvocoBackend.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class PostDTO
	{
		public int Id { get; set; }
		public int GroupId { get; set; }
		public int UserId { get; set; }
		public string Content { get; set; }
		public ICollection<PostComment> PostComments { get; set; }
	}
}
