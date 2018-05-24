using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class EventDTO
	{
		public int Id { get; set; }
		[Required]
		public string EventName { get; set; }
		public string EventDescription { get; set; }
		[Required]
		public DateTime EventDateTime { get; set; }
		public double? EventLocationLat { get; set; }
		public double? EventLocationLng { get; set; }
	}
}
