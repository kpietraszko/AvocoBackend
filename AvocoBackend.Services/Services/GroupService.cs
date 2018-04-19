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
	public class GroupService : IGroupService
	{
		private readonly IRepository<Group> _groupRepository;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;

		public GroupService(IRepository<Group> groupRepository, IMapper mapper, IImageService imageService)
		{
			_groupRepository = groupRepository;
			_mapper = mapper;
			_imageService = imageService;
		}

		public ServiceResult<int> CreateGroup(CreateGroupDTO groupData)
		{
			var newGroup = _mapper.Map<Group>(groupData);
			_groupRepository.Insert(newGroup);
			var result = _imageService.SaveImages(newGroup.Id, groupData.GroupImage, "Groups");
			if (result.IsError)
			{
				return new ServiceResult<int>(result.Errors);
			}
			var paths = result.SuccessResult;
			_mapper.Map(paths, newGroup);
			_groupRepository.Update(newGroup);
			return new ServiceResult<int>(newGroup.Id);
		}
	}
}
