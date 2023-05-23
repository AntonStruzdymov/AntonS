using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Business
{
    public class UserService : IUSerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        public UserService (IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            
        }

        

        public async Task<UserDTO> AddUserAsync(string name, string surname, string email, string password)
        {
            var userRole = await _unitOfWork.AccessLevel.GetRoleByName("User");
            var user = new User()
            {
                Name = name,
                Email = email,
                Password = GetPasswordHash(password),
                Surname = surname,
                AccessLevel = userRole
            };
            var userEntry = await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDTO>(await _unitOfWork.User.GetByIdAsync(userEntry.Entity.Id));
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            return users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.User.FindByEmailAsync(email);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
           var user = await _unitOfWork.User.GetByIdAsync(id);
           return _mapper.Map<UserDTO?>(user); 
        }

        public async Task<bool> IsPasswordCorrectAsync(string email, string password)
        {
            if(await IsUserExistsAsync(email))
            {
                var passwordHash = GetPasswordHash(password);
                var user = await _unitOfWork.User.IsPasswordCorrect(email,passwordHash);
                if(user != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new ArgumentException("User with that email does not exist", nameof(email));
            }
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            var user = await _unitOfWork.User.IsUserExistsAsync(email);
            return user;
        }

        public async Task RemoveUser(int id)
        {
            await _unitOfWork.User.Remove(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int id, List<PatchDTO> patchDtos)
        {
            await _unitOfWork.User.PatchAsync(id, patchDtos);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetPasswordHash(string password)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var encoding = Encoding.UTF8;
                var result = hash
                    .ComputeHash(
                        encoding
                            .GetBytes($"{password}{_configuration["Secrets:Salt"]}"));

                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
