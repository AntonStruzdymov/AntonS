using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AutoMapper;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;


namespace AntonS.Business
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<List<ArticleDTO>> GetArticlesByPageAsync(int pageNumber, int pageSize)
        {
            var articles = await _unitOfWork.Articles.GetArticlesByPage(pageNumber, pageSize);
            return articles.Select(a=> _mapper.Map<ArticleDTO>(a)).ToList();
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
        public async Task<List<ArticleDTO>> GetFullArticleContentAsync(List<ArticleDTO> articles)
        {
            var bag = new ConcurrentBag<ArticleDTO>();
            await Parallel.ForEachAsync(articles, async (dto, token) =>
            {
                var content = await GetArticleContentAsync(dto.ArticleSourceURL, dto.SourceID);
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
                    var nodesForDelete = textNode1.SelectNodes("//div[@class = 'news-widget']");
                    if (nodesForDelete != null)
                    {
                        foreach(var node in nodesForDelete)
                        {
                            node.RemoveAllChildren();
                            textNode1.RemoveClass(node.XPath);
                        }
                    }                    
                    var content1 = textNode1.InnerHtml;                    
                    return content1;
                case 2:
                    var textNode2 = doc.DocumentNode.SelectSingleNode("//div[@class = 'widget post']");
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
        public async Task<List<ArticleDTO>> GetUnratedArticles()
        {
            var unratedArticles = (await _unitOfWork.Articles.GetAllAsync()).Where(a => a.PositivityRating.Equals(0)).ToList();
            return unratedArticles.Select(a => _mapper.Map<ArticleDTO>(a)).ToList();
        }
        public async Task RemoveById(int id)
        {
            await _unitOfWork.Articles.Remove(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAll()
        {
            await _unitOfWork.Articles.DeleteAllArticles();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateArticlesAsync(int id, List<PatchDTO> patchDtos)
        {
            await _unitOfWork.Articles.PatchAsync(id, patchDtos);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<double?> Rate(ArticleDTO article)
        {
            if (string.IsNullOrEmpty(article.FullText))
            {
                throw new ArgumentException("Article or article text doesn't exist",
                    nameof(article));
            }
            else
            {
                Dictionary<string, int>? dict;
                using (var jsonReader = new StreamReader(@"C:\Users\Asus\Desktop\Антон\Project\AntonS\csvjson.json"))
                {
                    var jsonDict = await jsonReader.ReadToEndAsync();
                    dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonDict);
                }
                var clearText = PrepareText(article.FullText);
                var arr = clearText.Split(" ").ToList();                
                double summary = 0;
                foreach (var word in arr)
                {
                    if (dict.ContainsKey(word))
                    {
                        summary += dict[word];
                    }
                }
                double rating = ((summary*1000) / Convert.ToDouble(arr.Count));
                return Math.Round(rating,2);
                //using (var httpClient = new HttpClient())
                //{
                //    httpClient
                //            .DefaultRequestHeaders
                //            .Accept
                //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //    var request = new HttpRequestMessage(HttpMethod.Post,
                //            "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=15031bb039d704a3af5d07194f427aa3bf297058")
                //    {
                //        Content = new StringContent("[{\"text\":\"" + article.FullText + "\"}]",
                //                Encoding.UTF8, "application/json")
                //    };

                //    request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                //    var response = await httpClient.SendAsync(request);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseString = await response.Content.ReadAsStringAsync();
                //        var lemmas = JsonConvert.DeserializeObject<Root[]>(responseString)
                //            .SelectMany(root => root.Annotations.Lemma).Select(lemma => lemma.Value).ToArray();

                //        if (lemmas.Any())
                //        {
                //            var totalRate = lemmas
                //                .Where(lemma => dict.ContainsKey(lemma))
                //                .Aggregate<string, double>(0, (current, lemma)
                //                    => current + dict[lemma]);

                //            totalRate = totalRate / lemmas.Count();
                //            return totalRate;
                //        }
                //    }
                //}
            }            
        }
        public async Task AddRating(double? rating, int id)
        {
            await _unitOfWork.Articles.PatchAsync(id, new List<PatchDTO>()
            {
                new PatchDTO
                {
                    Name= nameof(ArticleDTO.PositivityRating),
                    Value = rating
                }
            });
            await _unitOfWork.SaveChangesAsync();
        }
        private string? PrepareText(string articleText)
        {
            articleText = articleText.Trim();

            articleText = Regex.Replace(articleText, "<.*?>", string.Empty);
            return articleText;
        }
    }
}