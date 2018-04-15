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
		}
	}

}
