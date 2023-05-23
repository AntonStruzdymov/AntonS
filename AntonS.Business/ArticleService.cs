using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AutoMapper;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Serialization;

namespace AntonS.Business
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
        }

        public async Task AddAsync(ArticleDTO article)
        {
            await _unitOfWork.Articles.AddAsync(_mapper.Map<Article>(article));
            await _unitOfWork.SaveChangesAsync();
            
        }


        public async Task<List<ArticleDTO>> GetAllArticlesAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync();
            return articles.Select(a => _mapper.Map<ArticleDTO>(a)).ToList();
        }

        public async Task<ArticleDTO?> GetArticleByIdWithSourceNameAsync(int id)
        {
            var artIdWithSourceName = await _unitOfWork.Articles.GetByIdAsync(id);
            return _mapper.Map<ArticleDTO>(artIdWithSourceName);
        }

        public async Task<List<ArticleDTO>> GetArticlesByNameAsync(string name)
        {
            var articlesByName = await _unitOfWork.Articles.GetArticlesByName(name);
            return articlesByName.Select(a => _mapper.Map<ArticleDTO>(a)).ToList();
        }

        public async Task<List<ArticleDTO>> GetArticlesFromRssSourceAsync(SourceDTO source, CancellationToken token)
        {
            var articles = new ConcurrentBag<ArticleDTO>();
            var urls = await GetArticleUrlsBySourceAsync(source.Id);
            using (var reader = XmlReader.Create(source.SourceRssURl))
            {
                var feed = SyndicationFeed.Load(reader);
                await Parallel.ForEachAsync(feed.Items.Where(item => !urls.Contains(item.Id)).ToArray(), token,
                    (item, token) =>
                    {
                        articles.Add(new ArticleDTO()
                        {
                            ArticleSourceURL = item.Id,
                            SourceID = source.Id,
                            Title = item.Title.Text,
                            Description = item.Summary.Text,
                            
                        });
                        return ValueTask.CompletedTask;
                    });
                reader.Close();
            }
            return articles.ToList();
        }
        public async Task<List<ArticleDTO>> GetFullArticleContentAsync (List<ArticleDTO> articles)
        {
            var bag = new ConcurrentBag<ArticleDTO>();
            await Parallel.ForEachAsync(articles, async (dto, token) =>
            {
                var content = await GetArticleContentAsync(dto.ArticleSourceURL,dto.SourceID);
                dto.FullText = content;
                bag.Add(dto);
            });
            return bag.ToList();
        }

        public async Task<int> GetTotalArticlesCountAsync()
        {
            var total = await _unitOfWork.Articles.CountAsync();
            return total;
        }
        private async Task<string[]> GetArticleUrlsBySourceAsync(int sourceId)
        {
            var articlesURLs = await _unitOfWork.Articles.GetAsQueryable()
                .Where(a => a.SourceID.Equals(sourceId))
                .Select(a => a.ArticleSourceURL)
                .ToArrayAsync();
            return articlesURLs;
           
        }
        private async Task<string> GetArticleContentAsync(string url, int sourceId)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            switch (sourceId)
            {
                case 1:
                    var textNode1 = doc.DocumentNode.SelectSingleNode("//div[@class = 'news-text']");
                    var content1 = textNode1.InnerHtml;
                    return content1;
                case 2:
                    var textNode2 = doc.DocumentNode.SelectSingleNode("//div[@class=\"card__body\"]");
                    var content2 = textNode2.InnerHtml;
                    return content2;
            }
            return null;
        }

        public async Task AddRangeAsync(List<ArticleDTO> articles)
        {
           await _unitOfWork.Articles.AddRangeAsync(articles.Select(a => _mapper.Map<Article>(a)));
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task RemoveById(int id)
        {
            await _unitOfWork.Articles.Remove(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateArticlesAsync(int id, List<PatchDTO> patchDtos)
        {
            await _unitOfWork.Articles.PatchAsync(id, patchDtos);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}