using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class GroupDTO
	{
		public int Id { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public string GroupPicture { get; set; } //TODO: jaki typ?
	}
}
