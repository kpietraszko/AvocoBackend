using AvocoBackend.Data.DTOs;
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
	 }
}
