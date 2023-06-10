using AntonDB.Entities;
using AntonS.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntonDB
{
    public class AntonDBContext : DbContext
    {
        public DbSet<Article> Articles { get;set; }
        public DbSet<Comment> Comments { get;set; }
        public DbSet<Source> Sources { get;set; }
        public DbSet<User> Users { get;set; }
        public DbSet<AccessLevel> AccessLevels { get;set; }
        public DbSet<RefreshToken> RefreshTokens { get;set; }

        public AntonDBContext(DbContextOptions<AntonDBContext> options) : base(options)
        {

        }

    
    }
}