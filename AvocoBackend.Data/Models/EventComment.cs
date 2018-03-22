using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class EventComment
	{
		[Key]
		public int CommentId { get; set; }
		public int EventId { get; set; }
		public Event Event { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public string Content { get; set; }

	}
}
