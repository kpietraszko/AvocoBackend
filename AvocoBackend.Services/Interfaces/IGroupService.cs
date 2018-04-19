﻿using AvocoBackend.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services.Interfaces
{
    public interface IGroupService
    {
		ServiceResult<int> CreateGroup(CreateGroupDTO createGroupDTO);
    }
}
