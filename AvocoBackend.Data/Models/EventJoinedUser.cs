using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
    public class EventJoinedUser
    {
		public int EventId { get; set; }
		public Event Event { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
