﻿using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class ArticleRepository : Repository <Article>, IArticleRepository
    {
        public ArticleRepository(AntonDBContext dbcontext): base (dbcontext) { }

        public async Task DeleteAllArticles()
        {
            var articles = await _context.Articles.AsQueryable().ToListAsync();
            _context.RemoveRange(articles);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Article>> GetArticlesByName(string name)
        {
           var articles = _context.Articles.Where(a=> a.Title.Contains(name)).ToList();
           return articles;
        }

        public async Task<List<Article>> GetArticlesByPage(int pageNumber, int pageSize)
        {
           var articles = await _context.Articles.Skip(pageNumber*pageSize).Take(pageSize).ToListAsync();
            return articles;
        }
    }
}
