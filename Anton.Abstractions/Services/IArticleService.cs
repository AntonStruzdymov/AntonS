using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Services
{
    public interface IArticleService
    {
        
        Task<ArticleDTO?> GetArticleByIdWithSourceNameAsync(int id);
        Task AddAsync(ArticleDTO article);
        Task<int> GetTotalArticlesCountAsync();
        Task<List<ArticleDTO>> GetAllArticlesAsync();
        Task<List<ArticleDTO>> GetArticlesByNameAsync(string name);
        Task<List<ArticleDTO>> GetArticlesFromRssSourceAsync(SourceDTO source, CancellationToken token);
        Task<List<ArticleDTO>> GetFullArticleContentAsync(List<ArticleDTO> articles);
        Task AddRangeAsync(List<ArticleDTO> articles);
        Task RemoveById(int id);
        Task UpdateArticlesAsync(int id, List<PatchDTO> patchDtos);

    }
}
