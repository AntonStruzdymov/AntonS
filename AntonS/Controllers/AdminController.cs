using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntonS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUSerService _userService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;

        public AdminController(IUSerService userService,
            IArticleService articleService,
            ICommentService commentService,
            ISourceService sourceService,
            IMapper mapper)
        {
            _userService = userService;
            _articleService = articleService;
            _commentService = commentService;
            _sourceService = sourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var sources = (await _sourceService.GetSourcesAsync())
                .Where(s => !string.IsNullOrEmpty(s.SourceRssURl))
                .ToArray();
            foreach (var source in sources)
            {
                var articleFromRss = (await _articleService.GetArticlesFromRssSourceAsync(source, CancellationToken.None));
                var articleFullContent = await _articleService.GetFullArticleContentAsync(articleFromRss);
                await _articleService.AddRangeAsync(articleFullContent);
            }
            return RedirectToAction("Index", "Article");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeEntity(AdminMainPageModel model)
        {
            var entityName = model.EntityName;

            switch (entityName)
            {
                case "User":
                    return RedirectToAction("EditUsers");
                case "Article":
                    return RedirectToAction("EditArticles");
                case "Comment":
                    return RedirectToAction("EditComments");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditUsers()
        {
            var usersDto = await _userService.GetAllUsersAsync();
            var users = usersDto.Select(u => _mapper.Map<UsersListModel>(u)).ToList();
            return View(new UsersListModel()
            {
                Users = users
            });
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(UsersListModel model)
        {
            var user = await _userService.GetUserByIdAsync(model.ChosenUserId);
            var userModel = _mapper.Map<UsersListModel>(user);
            return View("UserPreview", new ChangeUserModel()
            {
                Name = userModel.Name,
                Surname = userModel.Surname,
                Id = userModel.Id,
                Email = userModel.Email,
                Password = userModel.Password,
                AcessLevelID = userModel.AcessLevelID
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserModel model, int id)
        {

            if (model.DeleteUser)
            {
                await _userService.RemoveUser(id);
            }
            else
            {
                var valuesToPatch = new List<PatchDTO>();
                var modelValues = model.GetType().GetProperties();
                foreach (var property in modelValues)
                {
                    if (property.GetValue(model) != null)
                    {
                        var patchDto = new PatchDTO();
                        patchDto.Name = property.Name;
                        patchDto.Value = property.GetValue(model);
                        valuesToPatch.Add(patchDto);
                    }
                }

                await _userService.UpdateUserAsync(id, valuesToPatch);
            }            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditArticles()
        {
            var articlesDto = await _articleService.GetAllArticlesAsync();
            var articles = articlesDto.Select(u => _mapper.Map<ArticlesListModel>(u)).ToList();
            return View(new ArticlesListModel()
            {
                Articles = articles
            });
        }
        [HttpGet]
        public async Task<IActionResult> EditArticle(ArticlesListModel model)
        {
            var article = await _articleService.GetArticleByIdWithSourceNameAsync(model.Id);
            var articleModel = _mapper.Map<ArticlesListModel>(article);
            return View("ArticlePreview", new ChangeArticleModel()
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                FullText = article.FullText,
                PositivityRating = article.PositivityRating,
                SourceID = article.SourceID
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeArticle(ChangeArticleModel model, int id)
        {

            if (model.DeleteArticle)
            {
                await _articleService.RemoveById(id);
            }
            var valuesToPatch = new List<PatchDTO>();
            var modelValues = model.GetType().GetProperties();
            foreach (var property in modelValues)
            {
                if (property.GetValue(model) != null)
                {
                    var patchDto = new PatchDTO();
                    patchDto.Name = property.Name;
                    patchDto.Value = property.GetValue(model);
                    valuesToPatch.Add(patchDto);
                }
            }

            await _articleService.UpdateArticlesAsync(id, valuesToPatch);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditComments()
        {
            var articlesDto = await _articleService.GetAllArticlesAsync();
            var articles = articlesDto.Select(u => _mapper.Map<ArticlesListModel>(u)).ToList();
            return View(new ArticlesListModel()
            {
                Articles = articles
            });
        }

    }
}
