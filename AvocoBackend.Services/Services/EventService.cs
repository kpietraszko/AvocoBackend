using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using AvocoBackend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public class EventService : IEventService
	{
		private readonly IRepository<Event> _eventRepository;
		private readonly IMapper _mapper;

		public EventService(IRepository<Event> eventRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
		}
		public ServiceResult<int> CreateEvent(EventDTO eventData, int groupId)
		{
			var mappedEvent = _mapper.Map<Event>(eventData);
			mappedEvent.GroupId = groupId;
			_eventRepository.Insert(mappedEvent);
			return new ServiceResult<int>(mappedEvent.Id);
		}
	}
}
