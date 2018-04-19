using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Group :BaseModel
	{
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public string GroupPicture { get; set; }
		public ICollection<GroupJoinedUser> GroupJoinedUsers { get; set; }
		public ICollection<Event> Events { get; set; }
		public ICollection<GroupInterest> GroupInterests { get; set; }
	}
}
