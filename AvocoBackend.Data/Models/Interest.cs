using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Interest
	{
		public int InterestId { get; set; }
		public string InterestName { get; set; }
		public ICollection<GroupInterest> GroupsInterest { get; set; }
		public ICollection<UserInterest> UsersInterest { get; set; }
	}
}
