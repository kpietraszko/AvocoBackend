using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
	public class EventDTO
	{
		public int Id { get; set; }
		public string EventName { get; set; }
		public string EventDescription { get; set; }
		public DateTime EventDateTime { get; set; }
		public double EventLocationLat { get; set; }
		public double EventLocationLng { get; set; }
	}
}
