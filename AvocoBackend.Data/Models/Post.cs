	using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Post :BaseModel
	{
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public string Content { get; set; }
		public ICollection<PostComment> PostComments { get; set; }
	}
}
