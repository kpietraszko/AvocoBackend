using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class User
	{
		public User(string emailAdress) //HACK: usunać konstruktor
		{
			EmailAddress = emailAdress;
		}
		public User()
		{
		}
		public int UserId { get; set; }
		[Required]
		public string EmailAddress { get; set; }
		[Required]
		public string PasswordHash { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public int? Region { get; set; }
		public byte[] ProfileImage { get; set; }
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
