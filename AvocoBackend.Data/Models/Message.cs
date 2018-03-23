using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Message
	{
		public int Messageid { get; set; }
		public string MessageContent { get; set; }
		public int SenderUserId { get; set; }
		public User SenderUser { get; set; }
		public int RecipientUserId { get; set; }
		//public User RecipientUser { get; set; }
	}
}
