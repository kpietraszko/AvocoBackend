using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using AvocoBackend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public class GroupService : IGroupService
	{
		private readonly IRepository<Group> _groupRepository;
		private readonly IRepository<GroupInterest> _groupInterestRepository;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;

		public GroupService(IRepository<Group> groupRepository, IRepository<GroupInterest> groupInterestRepository, IMapper mapper, IImageService imageService)
		{
			_groupRepository = groupRepository;
			_groupInterestRepository = groupInterestRepository;
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

		public ServiceResult<GroupDTO> GetGroupInfo(int groupId)
		{
			var dbGroup = _groupRepository.GetBy(g => g.Id == groupId);
			if (dbGroup != null)
			{
				var groupInfo = _mapper.Map<GroupDTO>(dbGroup);
				return new ServiceResult<GroupDTO>(groupInfo);
			}
			return new ServiceResult<GroupDTO>("Group doesn't exist");
		}

		public ServiceResult<InterestDTO[]> GetGroupInterests(int groupId)
		{
			var dbGroup = _groupRepository.GetBy(g => g.Id == groupId);
			if (dbGroup == null)
			{
				return new ServiceResult<InterestDTO[]>("Group doesn't exist");
			}
			var groupInterests = _groupInterestRepository.GetAllBy(gi => gi.GroupId == groupId,
				gi => gi.Interest, gi => gi.Group); //includes
			var interests = groupInterests.Select(gi => new InterestDTO {Id = gi.InterestId, InterestName = gi.Interest.InterestName}).ToArray();
			return new ServiceResult<InterestDTO[]>(interests);
		}

		public ServiceResult<byte[]> GetImage(int groupId)
		{
			var dbGroup = _groupRepository.GetBy(g => g.Id == groupId);
			if (dbGroup == null)
			{
				return new ServiceResult<byte[]>("Group doesn't exist");
			}
			var path =  dbGroup.GroupPicture;
			if (path == null)
			{
				return new ServiceResult<byte[]>("Group doesn't have a photo");
			}
			var result = _imageService.GetImage(path);
			return result.IsError ? new ServiceResult<byte[]>(result.Errors) :
				new ServiceResult<byte[]>(result.SuccessResult);
		}
	}
}
