using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            //DatabaseConfig

            string dbConnectionString = configuration.GetConnectionString("Default") ?? string.Empty;
            var blobConnectionString = configuration["AzureStorageConnectionString"];

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

            //Services Configuration
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IHashtagsService, HashtagsService>();
            services.AddScoped<IStoriesService, StoriesService>();
            services.AddScoped<IFilesService>(s => new FilesService(blobConnectionString));
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<IAdminService, AdminService>();


            //identity configuration
            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                //Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Auth:Google:ClientId"] ?? "";
                    options.ClientSecret = configuration["Auth:Google:ClientSecret"] ?? "";
                    options.CallbackPath = "/signin-google";
                }).AddGitHub(options =>
                {
                    options.ClientId = configuration["Auth:GitHub:ClientId"] ?? "";
                    options.ClientSecret = configuration["Auth:GitHub:ClientSecret"] ?? "";
                    options.CallbackPath = "/signin-github";
                });

            services.AddAuthorization();

            services.AddSignalR();

            return services;
        }
    }
}
