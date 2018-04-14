using AutoMapper;
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
			CreateMap<User, UserInfo>();
			CreateMap<UserInfo, User>();
		}
    }
}
