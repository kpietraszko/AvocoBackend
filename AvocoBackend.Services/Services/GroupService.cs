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
	public class GroupService : IGroupService
	{
		private readonly IRepository<Group> _groupRepository;
		private readonly IRepository<GroupInterest> _groupInterestRepository;
		private readonly IRepository<Post> _postRepository;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;
		private readonly IClaimsService _claimsService;

		public GroupService(IRepository<Group> groupRepository, IRepository<GroupInterest> groupInterestRepository, IRepository<Post> postRepository, IMapper mapper, IImageService imageService, IClaimsService claimsService)
		{
			_groupRepository = groupRepository;
			_groupInterestRepository = groupInterestRepository;
			_postRepository = postRepository;
			_mapper = mapper;
			_imageService = imageService;
			_claimsService = claimsService;
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

		public ServiceResult<PostDTO[]> AddPost(int groupId, string postContent, HttpContext httpContext)
		{
			if (String.IsNullOrWhiteSpace(postContent))
			{
				return new ServiceResult<PostDTO[]>("Post content is empty");
			}
			var dbGroup = _groupRepository.GetBy(g => g.Id == groupId);
			if (dbGroup == null)
			{
				return new ServiceResult<PostDTO[]>("Group doesn't exist");
			}
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);

			if (userId == null)
			{
				return new ServiceResult<PostDTO[]>("UserId not found in claims");
			}
			var newPost = new Post{ GroupId = groupId, UserId = (int)userId, Content = postContent};
			_postRepository.Insert(newPost);
			return GetGroupsPosts(groupId); //zwraca wszystkie posty tej grupy
		}

		public ServiceResult<PostDTO[]> GetGroupsPosts(int groupId)
		{
			var dbGroup = _groupRepository.GetBy(g => g.Id == groupId);
			if (dbGroup == null)
			{
				return new ServiceResult<PostDTO[]>("Group doesn't exist");
			}
			var groupsPosts = _postRepository.GetAllBy(p => p.GroupId == groupId).ToArray();
			foreach (var post in groupsPosts)
			{
				_postRepository.GetRelatedCollections(post, p => p.PostComments);
			}
			var mappedPosts = _mapper.Map<PostDTO[]>(groupsPosts);
			return new ServiceResult<PostDTO[]>(mappedPosts);
		}
	}
}
