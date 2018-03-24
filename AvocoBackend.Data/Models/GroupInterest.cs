using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class GroupInterest
	{
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public int InterestId { get; set; }
		public Interest Interest { get; set; }
	}
}