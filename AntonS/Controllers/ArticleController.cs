﻿using AntonDB.Entities;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace AntonS.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICommentService commentService;
        private readonly ISourceService sourceService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public ArticleController (IArticleService articleService, 
            ICommentService commentService, 
            ISourceService sourceService,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.articleService = articleService;
            this.commentService = commentService;
            this.sourceService = sourceService;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page=1)
        {
            var totalArticles = await articleService.GetTotalArticlesCountAsync();
            if (int.TryParse(configuration["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = totalArticles

                };
                var articleDTOs = await articleService.GetArticlesByPageAsync(page,pageSize);
                var articles = articleDTOs.Select(dto => mapper.Map<ArticleShortModel>(dto)).ToList();

                return View(new ArticlesListModel()
                {                    
                    pageInfo = pageInfo,
                    Articles = articles
                });

            }
            else
            {
                return BadRequest();
            }
            
        }        
        [HttpGet]
        public async Task<IActionResult> ArticleDetailsWithComments(int id)
        {
            var dto = await articleService.GetArticleByIdWithSourceNameAsync(id);
                var comments = await commentService.GetCommentsByArticleIdAsync(id);
                    var model = new ArticleWithCommentModel()
                    {
                        _articlePreview = new ArticlePreviewModel()
                        {
                            Id = dto.Id,
                            Title = dto.Title,
                            FullText = dto.FullText,
                        },
                        _comments = comments.Select(c => new CommentModel()
                        {
                            _comment = c.CommentText,
                            _userName = c.AuthorName,
                            _articleID = c.ArticleID
                        }).ToList(),
                        _comment = new CommentModel() 
                        { 
                            _articleID = id
                        }

                    };
                    return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchedArticlesList(ArticlesListModel articlesListModel)
        { 
            string articleName = articlesListModel.Searched;
            var dtos = await articleService.GetArticlesByNameAsync(articleName);
            var articles = dtos.Select(dto => mapper.Map<ArticleShortModel>(dto)).ToList();
            return View(new ArticlesListModel()
            {
                Articles = articles
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ArticleCreateModel()
            {
                AvailableSources = (await sourceService.GetSourcesAsync())
                .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
                .ToList()
            };
            return View(model);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateModel articleCreateModel)
        {
            var newArticle = new Article()
            {
                Title = articleCreateModel.Title,
                Description = articleCreateModel.Description,
                FullText = articleCreateModel.FullText,
                SourceID = articleCreateModel.SourceID
            };
            var newArticleDTO = mapper.Map<ArticleDTO>(newArticle);
            await articleService.AddAsync(newArticleDTO);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddComment (ArticleWithCommentModel model)
        {
            var commentDto = new CommentDTO()
            {
                ArticleID = model._comment._articleID,
                AuthorName = model._comment._userName,
                CommentText = model._comment._comment
            };
            await commentService.AddComment(commentDto);
            return RedirectToAction("ArticleDetailsWithComments", new {id = model._comment._articleID});            
        }
        public async Task<IActionResult> Sort(ArticlesListModel model, int page=1)
        {
            var articles = await articleService.GetAllArticlesAsync();
            if(int.TryParse(configuration["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = articles.Count

                };

                if (model.SortBy == "By Descending")
                {
                    var sortedArticles = articles.OrderByDescending(a => a.PositivityRating).ToList();
                    model.Articles = sortedArticles.Select(s => mapper.Map<ArticleShortModel>(s))
                        .Skip(pageSize*(page-1))
                        .Take(pageSize).ToList();
                }
                else
                {
                    var sortedArticles = articles.OrderBy(a => a.PositivityRating).ToList();
                    model.Articles = sortedArticles.Select(s => mapper.Map<ArticleShortModel>(s))
                        .Skip(pageSize*(page-1))
                        .Take(pageSize).ToList();
                }
                return View("Index", new ArticlesListModel()
                {
                    Articles = model.Articles,
                    pageInfo = pageInfo
                });
            }
            else
            {
                return BadRequest();
            }
        }        
    }
}
