using AvocoBackend.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Interfaces
{
	public interface IEventService
	{
		ServiceResult<int> CreateEvent(EventDTO eventData, int groupId);
	}
}
