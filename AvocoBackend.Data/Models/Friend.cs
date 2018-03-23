using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Friend
	{
		public int? User1Id { get; set; }
		public User User1 { get; set; }
		//public int? User2Id { get; set; }
		//public User User2 { get; set; }
		public int SecondUserId { get; set; }
	}
}
