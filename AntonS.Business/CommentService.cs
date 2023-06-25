using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Business
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper= mapper;
        }
        public async Task<List<CommentDTO>> GetCommentsByArticleIdAsync(int articleId)
        {
            var comments = await _unitOfWork.Comments.GetCommentsByArticleId(articleId); 
            return comments.Select(c => _mapper.Map<CommentDTO>(c)).ToList();
        }
        public async Task AddComment(CommentDTO comment)
        {
            await _unitOfWork.Comments.AddAsync(_mapper.Map<Comment>(comment));
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<CommentDTO> GetCommentById(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            return _mapper.Map<CommentDTO>(comment);
        }
        public async Task DeleteComment (int id)
        {
            await _unitOfWork.Comments.Remove(id);
        }
        public async Task UpdateCommentAsync(int id, List<PatchDTO> list)
        {
            await _unitOfWork.Comments.PatchAsync(id, list);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
