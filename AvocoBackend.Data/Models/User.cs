using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string EmailAddress { get; set; }
		public string PasswordHash { get; set; } 
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Region { get; set; }
		public ICollection<GroupJoinedUser> GroupsJoinedUser { get; set; }
		public ICollection<EventJoinedUser> EventsJoinedUser { get; set; }
		//[InverseProperty("SenderUser")]
		public ICollection<Message> SentMessages { get; set; }
		//[InverseProperty("RecipientUser")]
		public ICollection<Message> ReceivedMessages { get; set; }
		//[InverseProperty("User1")]
		public ICollection<Friend> FriendsInvited { get; set; }
		//[InverseProperty("User2")]
		public ICollection<Friend> FriendsInvitedBy { get; set; }
	}
}
