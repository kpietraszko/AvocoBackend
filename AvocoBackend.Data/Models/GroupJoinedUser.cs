using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class GroupJoinedUser
	{
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
