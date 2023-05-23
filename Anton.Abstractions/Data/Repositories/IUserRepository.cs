using AntonDB.Entities;
using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
              
        public Task<bool> IsUserExistsAsync(string email);
        public Task<User> IsPasswordCorrect(string email, string password);
        public Task<User> FindByEmailAsync(string email);
        
        
    }
}
