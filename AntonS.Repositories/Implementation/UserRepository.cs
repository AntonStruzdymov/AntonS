using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AntonDBContext context) : base (context) { }

        
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email.Equals(email)).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> IsPasswordCorrect(string email, string password)
        {
            return await _context.Users.Where(u => u.Email.Equals(email) && u.Password.Equals(password)).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
           var user = await _context.Users.Select(u => u.Email.Equals(email)).FirstOrDefaultAsync();
           return user;
        }
    }
}
