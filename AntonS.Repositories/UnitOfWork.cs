using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Abstractions.Services;
using AntonS.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntonS.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AntonDBContext dBContext;
        private readonly IArticleRepository articleRepository;
        private readonly ICommentRepository commentRepository;
        private readonly ISourceRepository sourceRepository;
        private readonly IUserRepository userRepository;
        private readonly IAccessLevelRepository accessLevelRepository;
        

        public UnitOfWork(AntonDBContext dBContext, 
            IArticleRepository articleRepository, 
            ICommentRepository commentRepository, 
            ISourceRepository sourceRepository, 
            IUserRepository userRepository, 
            IAccessLevelRepository accessLevelRepository)
        {
            this.dBContext = dBContext;
            this.articleRepository = articleRepository;
            this.commentRepository = commentRepository;
            this.sourceRepository = sourceRepository;
            this.userRepository = userRepository;
            this.accessLevelRepository = accessLevelRepository;
            
        }
        public IArticleRepository Articles => articleRepository;
        public ICommentRepository Comments => commentRepository;
        public ISourceRepository Sources => sourceRepository;
        public IUserRepository User => userRepository;
        public IAccessLevelRepository AccessLevel=> accessLevelRepository;
        


        public async Task<int> SaveChangesAsync()
        {
            return await dBContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            dBContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}