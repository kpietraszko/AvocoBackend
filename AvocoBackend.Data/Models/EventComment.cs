using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class EventComment
	{
		public int EventCommentId { get; set; }
		public int EventId { get; set; }
		public Event Event { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		[Required]
		public string Content { get; set; }

	}
}
