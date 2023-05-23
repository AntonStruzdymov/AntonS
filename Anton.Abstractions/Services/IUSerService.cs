using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Services
{
    public interface IUSerService
    {
        //Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> AddUserAsync(string name, string surname, string email, string password);
        Task<bool> IsUserExistsAsync(string email);
        Task<bool> IsPasswordCorrectAsync(string email, string password);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task RemoveUser(int id);
        Task UpdateUserAsync(int id, List<PatchDTO> patchDtos);

    }
}
