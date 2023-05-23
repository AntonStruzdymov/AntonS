using AntonDB.Entities;
using AntonS.Abstractions.Data.Repositories;


namespace AntonS.Abstractions
{
    public interface IUnitOfWork
    {
        public IArticleRepository Articles { get; }
        public ICommentRepository Comments { get; }
        public ISourceRepository Sources { get; }
        public IAccessLevelRepository AccessLevel { get; }
        public IUserRepository User { get; }
        Task<int> SaveChangesAsync();
    }
}