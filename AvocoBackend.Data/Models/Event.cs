using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.Models
{
    public class Event
    {
		public int EventId { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public string EventName { get; set; }
		public string EventDescription { get; set; }
		public DateTime EventDateTime { get; set; }
		public double EventLocationLat { get; set; }
		public double EventLocationLng { get; set; }
	}
}
