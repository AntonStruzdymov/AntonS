using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Services
{
    public interface ICommentService
    {
        Task<List<CommentDTO>> GetCommentsByArticleIdAsync(int articleId);
        Task AddComment(CommentDTO comment);
    }
}
