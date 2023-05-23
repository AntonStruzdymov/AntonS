using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Services
{
    public interface IAcessLevelService
    {
        Task SetDefaultRole();
        Task<bool> IsRoleExist(string roleName);
        Task<string> GetRoleName(int userId);
        Task<AccessLevelDTO> GetRoleByName(string roleName);
    }
}
