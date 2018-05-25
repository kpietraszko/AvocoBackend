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
	public class HomeService : IHomeService
	{
		private readonly IRepository<Post> _postRepository;
		private readonly IRepository<Friend> _friendRepository;
		private readonly IRepository<GroupJoinedUser> _groupsUsersRepository;
		private readonly IClaimsService _claimsService;
		private readonly IImageService _imageService;
		private readonly IMapper _mapper;

		public HomeService(IRepository<Post> postRepository, IRepository<Friend> friendRepository, IRepository<GroupJoinedUser> groupsUsersRepository,
			IClaimsService claimsService, IImageService imageService, IMapper mapper)
		{
			_postRepository = postRepository;
			_friendRepository = friendRepository;
			_groupsUsersRepository = groupsUsersRepository;
			_claimsService = claimsService;
			_imageService = imageService;
			_mapper = mapper;
		}

		public ServiceResult<PostDTO[]> GetFeedPosts(HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<PostDTO[]>("UserId not found in claims");
			}
			var posts = _postRepository.GetAll(p => p.User, p => p.Group)
				.Where(p => IsFriend((int)userId, p.UserId) || IsInUsersGroup((int)userId, p))
				.OrderByDescending(p => p.Id);
			var mappedPosts = _mapper.Map<PostDTO[]>(posts);
			foreach (var post in mappedPosts)
			{
				post.FirstName = post.User.FirstName;
				post.LastName = post.User.LastName;
				post.GroupName = post.Group.GroupName;
				if (post.User.ImageSmallPath != null)
				{
					var authorImage = _imageService.GetImage(post.User.ImageSmallPath);
					post.UserImage = authorImage.SuccessResult;
				}
			}

			return new ServiceResult<PostDTO[]>(mappedPosts);
		}
		private bool IsFriend(int thisUserId, int postUserId)
		{
			var friendResult = _friendRepository.GetBy(f => (f.User1Id == thisUserId && f.User2Id == postUserId) ||
				(f.User1Id == postUserId && f.User2Id == thisUserId));
			return friendResult != null;
		}
		private bool IsInUsersGroup(int userId, Post post)
		{
			var groupId = post.GroupId;
			var groupUserResult = _groupsUsersRepository.GetBy(gu => gu.GroupId == groupId && gu.UserId == userId);
			return groupUserResult != null;

		}
	}
}
