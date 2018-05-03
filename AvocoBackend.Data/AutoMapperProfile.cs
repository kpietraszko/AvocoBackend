using AutoMapper;
using AvocoBackend.Data.DTOs;
using AvocoBackend.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvocoBackend.Api
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserDTO>();

			CreateMap<UserDTO, User>()  //ForAllMembers nie można użyć
				.ForMember(u => u.Id, opts => opts.Ignore())
				.ForMember(u => u.FirstName, opts => opts.Condition(src => src.FirstName != null))
				.ForMember(u => u.FirstName, opts => opts.UseDestinationValue())
				.ForMember(u => u.LastName, opts => opts.Condition(src => src.LastName != null))
				.ForMember(u => u.LastName, opts => opts.UseDestinationValue())
				.ForMember(u => u.Region, opts => opts.Condition(src => src.Region != null))
				.ForMember(u => u.Region, opts => opts.UseDestinationValue()); //troche bez sensu

			CreateMap<RegisterDTO, User>();

			CreateMap<Group, GroupDTO>();

			CreateMap<Interest, InterestDTO>();

			CreateMap<ImagePathsDTO, User>()
				.ForMember(u => u.ImagePath, opts => opts.MapFrom(ip => ip.ImagePath))
				.ForMember(u => u.ImageSmallPath, opts => opts.MapFrom(ip => ip.ImageSmallPath))
				.ForAllOtherMembers(opts => opts.UseDestinationValue());

			CreateMap<CreateGroupDTO, Group>()
				.ForSourceMember(cg => cg.GroupImage, opts => opts.Ignore());

			CreateMap<ImagePathsDTO, Group>()
				.ForMember(g => g.GroupPicture, opts => opts.MapFrom(ip => ip.ImagePath))
				.ForAllOtherMembers(opts => opts.UseDestinationValue());

			CreateMap<Post, PostDTO>();
		}
	}

}
