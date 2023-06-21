using AntonDB.Entities;
using AntonS.Core.DTOs;
using AutoMapper;

namespace AntonS.MappingProfiles
{
    public class SourceProfile : Profile
    {
        public SourceProfile() 
        {
            CreateMap<Source, SourceDTO>();
            CreateMap<SourceDTO, Source>();
        }
    }
}
