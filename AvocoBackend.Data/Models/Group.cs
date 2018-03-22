using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Group
	{
		public int GroupId { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public ICollection<User> Users { get; set; }
		public ICollection<Event> Events { get; set; }
		public ICollection<Interest> GroupInterests { get; set; }
	}
}
