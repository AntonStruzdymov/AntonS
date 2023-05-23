using AntonS.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Data.Repositories
{
    public interface IAccessLevelRepository : IRepository<AccessLevel>
    {
        Task SetDefautlRole();
        Task<bool> IsRoleExists(string roleName);
        Task<string> GetRoleName(int userId);
        Task<AccessLevel> GetRoleByName(string roleName);
    }
}
