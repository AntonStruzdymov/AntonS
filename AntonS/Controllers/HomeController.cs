using AntonS.Abstractions.Services;
using AntonS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AntonS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, IMapper mapper)
        {
            _logger = logger;
            _articleService = articleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var articles = (await _articleService.GetAllArticlesAsync())
                .OrderByDescending(a=>a.PositivityRating)
                .Take(6)
                .ToList();
            var model = new ArticlesListModel()
            {
                Articles = articles.Select(a => _mapper.Map<ArticleShortModel>(a)).ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}