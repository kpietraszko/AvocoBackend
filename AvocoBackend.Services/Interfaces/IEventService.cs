using AvocoBackend.Data.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Interfaces
{
	public interface IEventService
	{
		ServiceResult<int> CreateEvent(EventDTO eventData, int groupId);
		ServiceResult<EventDTO> GetDetails(int eventId, HttpContext httpContext);
		ServiceResult<UserDTO[]> GetInterestedUsers(int eventId);
		ServiceResult<byte[]> GetGroupImage(int eventId);
		ServiceResult<bool> SetInterested(int eventId, bool interested, HttpContext httpContext);
		ServiceResult<EventDTO[]> GetUserEvents(int userId);
		ServiceResult<EventCommentDTO[]> GetEventComments(int eventId);
		ServiceResult<EventCommentDTO[]> AddComment(int eventId, string content, HttpContext httpContext);
	}
}
