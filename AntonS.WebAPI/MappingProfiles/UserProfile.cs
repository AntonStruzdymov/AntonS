using AntonDB.Entities;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;

namespace AntonS.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserDTO, UsersListModel>();
        }
    }
}
