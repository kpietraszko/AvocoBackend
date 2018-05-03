﻿using AvocoBackend.Data.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Interfaces
{
    public interface IGroupService
    {
		ServiceResult<int> CreateGroup(CreateGroupDTO createGroupDTO);
		ServiceResult<GroupDTO> GetGroupInfo(int groupId);
		ServiceResult<InterestDTO[]> GetGroupInterests(int groupId);
		ServiceResult<byte[]> GetImage(int groupId);
		ServiceResult<PostDTO[]> AddPost(int groupId, string postContent, HttpContext httpContext);
		ServiceResult<PostDTO[]> GetGroupsPosts(int groupId);
	 }
}
