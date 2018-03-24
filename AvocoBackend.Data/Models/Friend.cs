using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Friend
	{
		public int User1Id { get; set; }
		[InverseProperty("FriendsInvited")]
		public User User1 { get; set; }
		public int User2Id { get; set; }
		[InverseProperty("FriendsInvitedBy")]
		public User User2 { get; set; }
	}
}
