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
		private readonly IClaimsService _claimsService;
		private readonly IImageService _imageService;
		private readonly IGroupService _groupService;
		private readonly IMapper _mapper;

		public EventService(IRepository<Event> eventRepository, IRepository<EventJoinedUser> eventUsersRepository,
			IClaimsService claimsService, IImageService imageService, IGroupService groupService, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventUsersRepository = eventUsersRepository;
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
	}
}
