using AntonS.Abstractions.Data.Repositories;
using AntonS.Abstractions.Services;
using AntonS.Abstractions;
using AntonS.Repositories;
using AntonS.Repositories.Implementation;
using AntonS.Business;
using AntonDB;
using Microsoft.EntityFrameworkCore;

namespace AntonS.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AntonDBContext>(
               opt =>
               {
                   var connString = builder.Configuration
                       .GetConnectionString("DefaultConnection");
                   opt.UseSqlServer(connString);
               });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ISourceRepository, SourceRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccessLevelRepository, AccessLevelRepository>();

            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<ISourceService, SourceService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddTransient<IUSerService, UserService>();
            builder.Services.AddTransient<IAcessLevelService, AccessLevelService>();

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}