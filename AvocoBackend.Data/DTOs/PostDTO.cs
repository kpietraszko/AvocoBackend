using AvocoBackend.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class PostDTO
	{
		public int Id { get; set; }
		[JsonIgnore]
		public Group Group { get; set; }
		public int GroupId { get; set; }
		public string GroupName { get; set; }
		public int UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public byte[] UserImage { get; set; }
		public string Content { get; set; }
		public ICollection<CommentDTO> PostComments { get; set; }
	}
}
