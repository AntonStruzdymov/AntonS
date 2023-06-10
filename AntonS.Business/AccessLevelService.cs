using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.DB.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Business
{
    public class AccessLevelService : IAcessLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccessLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccessLevelDTO> GetRoleByName(string roleName)
        {
           var dto = await _unitOfWork.AccessLevel.GetRoleByName(roleName);
            return _mapper.Map<AccessLevelDTO>(dto);
        }

        public Task<string> GetRoleName(int userId)
        {
           return _unitOfWork.AccessLevel.GetRoleName(userId);
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _unitOfWork.AccessLevel.GetAsQueryable().AnyAsync(role => role.name.Equals(roleName));
        }

        public async Task SetDefaultRole()
        {
            var defaultRole = false;
            if(!await IsRoleExist("User"))
            {
                defaultRole = true;
                await _unitOfWork.AccessLevel.AddAsync(new AccessLevel() { name = "User" });
            }
            if (!await IsRoleExist("Admin"))
            {
                defaultRole = true;
                await _unitOfWork.AccessLevel.AddAsync(new AccessLevel() { name = "Admin" });
            }
            if (defaultRole)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
