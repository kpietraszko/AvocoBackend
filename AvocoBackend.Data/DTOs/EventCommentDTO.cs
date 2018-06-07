using AvocoBackend.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class EventCommentDTO
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		[JsonIgnore]
		public Event Event { get; set; }
		public int UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public byte[] Image { get; set; }
		public string Content { get; set; }
	}
}
