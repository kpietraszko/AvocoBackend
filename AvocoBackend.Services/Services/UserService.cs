using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using AvocoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AvocoBackend.Services.Services
{
	public enum ImageSize { Original, Small };
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<UserInterest> _userInterestRepository;
		private readonly IRepository<Interest> _interestRepository;
		private readonly IRepository<GroupJoinedUser> _groupJoinedRepository;
		private readonly IRepository<Friend> _friendRepository;
		private readonly IClaimsService _claimsService;
		private readonly IImageService _imageService;
		private readonly IMapper _mapper;

		public UserService(IRepository<User> userRepository, IRepository<UserInterest> userInterestRepository,
			IRepository<Interest> interestRepository, IRepository<GroupJoinedUser> groupJoinedRepository,
			IRepository<Friend> friendRepository,
			IClaimsService claimsService, IMapper mapper, IImageService imageService, IHostingEnvironment hostingEnvironment)
		{
			_userRepository = userRepository;
			_claimsService = claimsService;
			_mapper = mapper;
			_userInterestRepository = userInterestRepository;
			_interestRepository = interestRepository;
			_groupJoinedRepository = groupJoinedRepository;
			_friendRepository = friendRepository;
			_imageService = imageService;
		}

		public ServiceResult<UserDTO> GetUserInfo(int userId)
		{
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser != null)
			{
				var userInfo = _mapper.Map<UserDTO>(dbUser);
				return new ServiceResult<UserDTO>(userInfo);
			}
			return new ServiceResult<UserDTO>("User doesn't exist");
		}

		public ServiceResult<UserDTO> SetUserInfo(UserDTO userInfo, HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);

			if (userId == null)
			{
				return new ServiceResult<UserDTO>("UserId not found in claims");
			}
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser == null)
			{
				return new ServiceResult<UserDTO>("User doesn't exist");
			}
			_mapper.Map(userInfo, dbUser);
			_userRepository.Update(dbUser);
			return new ServiceResult<UserDTO>(userInfo);
		}

		public ServiceResult<InterestDTO[]> GetInterests(int userId) //dziala
		{
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser == null)
			{
				return new ServiceResult<InterestDTO[]>("User doesn't exist");
			}
			var userInterests = _userInterestRepository.GetAllBy(ui => ui.UserId == userId,
				ui => ui.Interest, ui => ui.User); //includes
			var interests = userInterests.Select(ui => new InterestDTO {InterestName = ui.Interest.InterestName}).ToArray();
			return new ServiceResult<InterestDTO[]>(interests);

		}
		public ServiceResult<InterestDTO> AddInterest(HttpContext httpContext, int? interestId = null, string interestName = null)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<InterestDTO>("UserId not found in claims");
			}
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser == null)
			{
				return new ServiceResult<InterestDTO>("User doesn't exist");
			}
			if (interestId != null)
			{
				if (_interestRepository.GetBy(i => i.Id == interestId) != null && //jesli zainteresowanie istnieje 
					_userInterestRepository.GetBy(ui => ui.UserId == userId && ui.InterestId == interestId) == null) //i ten uzytkownik go nie ma
				{
					_userInterestRepository.Insert(new UserInterest { UserId = (int)userId, InterestId = (int)interestId });
				}
				else return new ServiceResult<InterestDTO>("Interest doesn't exist or user already has it");

			}
			else
			{
				if (_interestRepository.GetBy(i => i.InterestName == interestName) == null) //jesli zainteresowanie o tej nazwie nie istnieje
				{
					var newInterest = new Interest { InterestName = interestName };
					_interestRepository.Insert(newInterest);
					_userInterestRepository.Insert(new UserInterest { Interest = newInterest, UserId = (int)userId });
				}
				else return new ServiceResult<InterestDTO>("Interest with that name exists");
			}
			return new ServiceResult<InterestDTO>((InterestDTO)null);

		}

		public ServiceResult<InterestDTO[]> SearchInterests(string searchText)
		{
			if (String.IsNullOrWhiteSpace(searchText))
				return new ServiceResult<InterestDTO[]>("searchText is empty");
			var found = _interestRepository.GetAllBy(i => i.InterestName.Contains(searchText));
			var result = found.Select(f => _mapper.Map<InterestDTO>(f));
			return new ServiceResult<InterestDTO[]>(result.ToArray());
		}

		public ServiceResult<UserDTO[]> GetFriends(HttpContext httpContext) //dziala
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<UserDTO[]>("UserId not found in claims");
			}
			var friends = _friendRepository.GetAllBy(f => f.User1Id == userId || f.User2Id == userId,
				f => f.User1, f => f.User2);
			var friendsData = new List<UserDTO>();
			foreach (var f in friends)
			{
				var userToAdd = f.User1Id == userId ? f.User2 : f.User1;
				friendsData.Add(_mapper.Map<UserDTO>(userToAdd));
			}
			return new ServiceResult<UserDTO[]>(friendsData.ToArray());
		}

		public ServiceResult<UserDTO[]> AddFriend(int user2Id, HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<UserDTO[]>("UserId not found in claims");
			}
			if (userId == user2Id)
			{
				return new ServiceResult<UserDTO[]>("User1 == User2");
			}
			var alreadyExists = _friendRepository.GetBy(f => (f.User1Id == userId && f.User2Id == user2Id) ||
			(f.User2Id == userId && f.User1Id == user2Id)) != null;
			if (alreadyExists)
			{
				return new ServiceResult<UserDTO[]>("Friendship already exists");
			}
			if (_userRepository.GetBy(u => u.Id == userId) == null ||
				_userRepository.GetBy(u => u.Id == user2Id) == null)
			{
				return new ServiceResult<UserDTO[]>("One of users doesn't exist");
			}
			_friendRepository.Insert(new Friend { User1Id = (int)userId, User2Id = user2Id });
			return GetFriends(httpContext);
		}
		public ServiceResult<GroupDTO[]> GetGroups(int userId) //dziala
		{
			if (_userRepository.GetBy(u => u.Id == userId) != null)
			{
				var groups = _groupJoinedRepository.GetAllBy(g => g.UserId == userId, g => g.Group);
				var groupsData = groups.Select(g => _mapper.Map<GroupDTO>(g.Group));
				return new ServiceResult<GroupDTO[]>(groupsData.ToArray());
			}
			return new ServiceResult<GroupDTO[]>("User doesn't exist");
		}
		public ServiceResult<UserDTO[]> Unfriend(int user2Id, HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<UserDTO[]>("UserId not found in claims");
			}
			if (user2Id == userId)
			{
				return new ServiceResult<UserDTO[]>("User1 == User2");
			}
			var user2Exists = _userRepository.GetBy(u => u.Id == user2Id) != null;
			if (!user2Exists)
			{
				return new ServiceResult<UserDTO[]>("User2 doesn't exist");
			}
			var friendship = _friendRepository.GetBy(f => (f.User1Id == userId && f.User2Id == user2Id) ||
					(f.User1Id == user2Id && f.User2Id == userId));
			if (friendship == null)
			{
				return new ServiceResult<UserDTO[]>("Friendship doesn't exist");
			}
			_friendRepository.Delete(friendship);
			return GetFriends(httpContext);
		}

		public ServiceResult<byte[]> GetImage(int userId, ImageSize imageSize)
		{
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser == null)
			{
				return new ServiceResult<byte[]>("User doesn't exist");
			}
			var path = imageSize == ImageSize.Original ? dbUser.ImagePath : dbUser.ImageSmallPath;
			if (path == null)
			{
				return new ServiceResult<byte[]>("User doesn't have a photo");
			}
			var result = _imageService.GetImage(path);
			return result.IsError ? new ServiceResult<byte[]>(result.Errors) :
				new ServiceResult<byte[]>(result.SuccessResult);
		}

		public ServiceResult<byte[]> SetImage(IFormFile image, HttpContext httpContext)
		{
			var userId = _claimsService.GetFromClaims<int?>(httpContext, ClaimTypes.Sid);
			if (userId == null)
			{
				return new ServiceResult<byte[]>("UserId not found in claims");
			}
			var dbUser = _userRepository.GetBy(u => u.Id == userId);
			if (dbUser == null)
			{
				return new ServiceResult<byte[]>("User doesn't exist");
			}
			var result = _imageService.SaveImages((int)userId, image, "Users");
			if (result.IsError)
			{
				return new ServiceResult<byte[]>(result.Errors);
			}
			var paths = result.SuccessResult;
			_mapper.Map(paths, dbUser);
			_userRepository.Update(dbUser);

			var getResult = GetImage((int)userId, ImageSize.Original);
			if (getResult.IsError)
			{
				return new ServiceResult<byte[]>(getResult.Errors);
			}
			return new ServiceResult<byte[]>(getResult.SuccessResult);
		}
	}
}
