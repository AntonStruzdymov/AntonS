using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Abstractions.Services;
using AntonS.Business;
using AntonS.DB.Entities;
using AntonS.Repositories;
using AntonS.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using AntonS.Controllers;

namespace AntonS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AntonDBContext>(
                opt =>
                {
                    var connString = builder.Configuration
                        .GetConnectionString("DefaultConnection");
                    opt.UseSqlServer(connString);
                });

            // Add services to the container.
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
            builder.Services.AddHangfire(x=> x.UseSqlServerStorage(builder.Configuration
                        .GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();
            
            builder.Services.AddControllersWithViews();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Login/Login");
                    options.AccessDeniedPath = new PathString("/Login/Login");
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<AdminController>("get news", a => a.GetNews(), Cron.Daily(16));
            RecurringJob.AddOrUpdate<AdminController>("rate news", a => a.RateArticles(), Cron.Daily(16));
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            
            
        }
    }
}