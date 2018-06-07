using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public class EventService : IEventService
	{
		private readonly IRepository<Event> _eventRepository;
		private readonly IRepository<EventJoinedUser> _eventUsersRepository;
		private readonly IRepository<EventComment> _eventCommentsRepository;
		private readonly IClaimsService _claimsService;
		private readonly IImageService _imageService;
		private readonly IGroupService _groupService;
		private readonly IMapper _mapper;

		public EventService(IRepository<Event> eventRepository, IRepository<EventJoinedUser> eventUsersRepository, IRepository<EventComment> eventCommentsRepository,
			IClaimsService claimsService, IImageService imageService, IGroupService groupService, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventUsersRepository = eventUsersRepository;
			_eventCommentsRepository = eventCommentsRepository;
			_claimsService = claimsService;
			_imageService = imageService;
			_groupService = groupService;
			_mapper = mapper;
		}
		public ServiceResult<int> CreateEvent(EventDTO eventData, int groupId)
		{
			var mappedEvent = _mapper.Map<Event>(eventData);
			mappedEvent.GroupId = groupId;
			_eventRepository.Insert(mappedEvent);
			return new ServiceResult<int>(mappedEvent.Id);
		}

		public ServiceResult<EventDTO> GetDetails(int eventId, HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<EventDTO>("UserId not found in claims");
			}
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId, e => e.Group);
			if (dbEvent == null)
			{
				return new ServiceResult<EventDTO>("Event doesn't exist");
			}
			var mappedEvent = _mapper.Map<EventDTO>(dbEvent);
			var userJoinedEvent = _eventUsersRepository.GetBy(e => e.EventId == eventId && e.UserId == userId);
			mappedEvent.Joined = userJoinedEvent != null;
			mappedEvent.GroupName = dbEvent.Group.GroupName;
			return new ServiceResult<EventDTO>(mappedEvent);
		}
		public ServiceResult<UserDTO[]> GetInterestedUsers(int eventId)
		{
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId, e => e.Group);
			if (dbEvent == null)
			{
				return new ServiceResult<UserDTO[]>("Event doesn't exist");
			}
			var interestedUsers = _eventUsersRepository.GetAllBy(e => e.EventId == eventId, e => e.User);
			var users = interestedUsers.Select(iu => iu.User);
			var mappedUsers = _mapper.Map<UserDTO[]>(users);
			foreach (var user in mappedUsers)
			{
				if (user.ImageSmallPath != null)
				{
					var imageResult = _imageService.GetImage(user.ImageSmallPath);
					if (!imageResult.IsError)
					{
						user.Image = imageResult.SuccessResult;
					}
				}
			}
			return new ServiceResult<UserDTO[]>(mappedUsers);
		}

		public ServiceResult<byte[]> GetGroupImage(int eventId)
		{
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId, e => e.Group);
			if (dbEvent == null)
			{
				return new ServiceResult<byte[]>("Event doesn't exist");
			}
			var imageResult = _groupService.GetImage(dbEvent.GroupId);
			if (imageResult.IsError)
			{
				return new ServiceResult<byte[]>("Group doesn't have an image");
			}
			return new ServiceResult<byte[]>(imageResult.SuccessResult);
		}

		public ServiceResult<bool> SetInterested(int eventId, bool interested, HttpContext httpContext)
		{
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId, e => e.Group);
			if (dbEvent == null)
			{
				return new ServiceResult<bool>("Event doesn't exist");
			}
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<bool>("UserId not found in claims");
			}
			var interestedResult = _eventUsersRepository.GetBy(e => e.EventId == eventId && e.UserId == userId);
			var dbInterested = interestedResult != null;
			if (interested)
			{
				if (dbInterested)
				{
					return new ServiceResult<bool>("User already interested");
				}
				_eventUsersRepository.Insert(new EventJoinedUser { EventId = eventId, UserId = (int)userId });
				return new ServiceResult<bool>(true);
			}
			if (!dbInterested)
			{
				return new ServiceResult<bool>("User already uninterested");
			}
			_eventUsersRepository.Delete(e => e.EventId == eventId && e.UserId == userId);
			return new ServiceResult<bool>(false);
		}

		public ServiceResult<EventDTO[]> GetUserEvents(int userId)
		{
			var events = _eventUsersRepository.GetAllBy(e => e.UserId == userId, e => e.Event, e => e.Event.Group)
				.Select(e => e.Event);
			var mappedEvents = _mapper.Map<EventDTO[]>(events);
			foreach (var ev in mappedEvents)
			{
				ev.GroupName = events.FirstOrDefault(e => e.Id == ev.Id).Group.GroupName;
			}
			return new ServiceResult<EventDTO[]>(mappedEvents);
		}

		public ServiceResult<EventCommentDTO[]> GetEventComments(int eventId)
		{
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId);
			if (dbEvent == null)
			{
				return new ServiceResult<EventCommentDTO[]>("Event doesn't exist");
			}
			var comments = _eventCommentsRepository.GetAllBy(c => c.EventId == eventId, c => c.User);
			var mappedComments = _mapper.Map<EventCommentDTO[]>(comments);
			foreach (var comment in mappedComments)
			{
				if (comment.User.ImageSmallPath != null)
				{
					var imageResult = _imageService.GetImage(comment.User.ImageSmallPath);
					if (!imageResult.IsError)
					{
						comment.Image = imageResult.SuccessResult;
					}
					comment.FirstName = comment.User.FirstName;
					comment.LastName = comment.User.LastName;
				}
			}
			return new ServiceResult<EventCommentDTO[]>(mappedComments);
		}

		public ServiceResult<EventCommentDTO[]> AddComment(int eventId, string content, HttpContext httpContext)
		{
			if (String.IsNullOrWhiteSpace(content))
			{
				return new ServiceResult<EventCommentDTO[]>("Comment can't be empty");
			}
			var dbEvent = _eventRepository.GetBy(e => e.Id == eventId);
			if (dbEvent == null)
			{
				return new ServiceResult<EventCommentDTO[]>("Event doesn't exist");
			}
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<EventCommentDTO[]>("UserId not found in claims");
			}
			var newComment = new EventComment { EventId = eventId, UserId = (int)userId, Content = content};
			_eventCommentsRepository.Insert(newComment);
			return GetEventComments(eventId);
		}
	}
}
