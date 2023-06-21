using AntonDB.Entities;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;

namespace AntonS.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile() 
        {
            CreateMap<Article, ArticleDTO>();
            CreateMap<ArticleDTO, Article>();
            CreateMap<ArticleDTO, ArticleShortModel>();
            CreateMap<ArticleDTO, ArticlePreviewModel>();
        }
    }
}
