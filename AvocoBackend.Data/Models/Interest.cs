using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Interest :BaseModel
	{
		public string InterestName { get; set; }
		public ICollection<GroupInterest> GroupsInterest { get; set; }
		public ICollection<UserInterest> UsersInterest { get; set; }
	}
}
