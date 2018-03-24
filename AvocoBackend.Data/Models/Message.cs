using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AvocoBackend.Data.Models
{
	public class Message
	{
		public int MessageId { get; set; }
		public string MessageContent { get; set; }
		public int SenderUserId { get; set; }
		[InverseProperty("SentMessages")]
		public User SenderUser { get; set; }
		public int RecipientUserId { get; set; }
		[InverseProperty("ReceivedMessages")]
		public User RecipientUser { get; set; }
	}
}
