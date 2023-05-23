using AntonDB;
using AntonS.Abstractions.Data.Repositories;
using AntonS.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class AccessLevelRepository : Repository<AccessLevel>, IAccessLevelRepository
    {
        public AccessLevelRepository(AntonDBContext context) : base (context) 
        {
        }

        public async Task<AccessLevel> GetRoleByName(string roleName)
        {
           var acLvl = await _context.AccessLevels.Where(a => a.name.Equals(roleName)).FirstOrDefaultAsync();
            return acLvl;
        }

        public async Task<string> GetRoleName(int userId)
        {
            var user = _context.Users.Where(u=>u.Id.Equals(userId)).FirstOrDefault();
            var role = await _context.AccessLevels.Where(a=> a.Id.Equals(user.AccessLevelid)).FirstOrDefaultAsync();
            return role.name;
        }

        public Task<bool> IsRoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task SetDefautlRole()
        {
            throw new NotImplementedException();
        }
    }
}
