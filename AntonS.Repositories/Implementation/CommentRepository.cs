using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AntonDBContext context) : base (context) { }

        public async Task<List<Comment>> GetCommentsByArticleId(int id)
        {
           var comments = await _context.Comments.Where(c=> c.ArticleID.Equals(id)).AsQueryable().ToListAsync();
           return comments;
        }
    }
}
