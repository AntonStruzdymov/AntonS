using AntonDB.Entities;
using AntonS.Core.DTOs;
using AutoMapper;

namespace AntonS.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile() 
        {
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();
        }
    }
}
