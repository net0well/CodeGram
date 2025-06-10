
using CodeGram.Data;
using CodeGram.Data.Helpers;
using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DatabaseConfig

string dbConnectionString = builder.Configuration.GetConnectionString("Default") ?? string.Empty;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));


//Services Configuration
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IHashtagsService, HashtagsService>();
builder.Services.AddScoped<IStoriesService, StoriesService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();


//identity configuration
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"] ?? "";
        options.CallbackPath = "/signin-google";
    }).AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["Auth:GitHub:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Auth:GitHub:ClientSecret"] ?? "";
        options.CallbackPath = "/signin-github";
    });

builder.Services.AddAuthorization();

builder.Services.AddSignalR();

var app = builder.Build();

//Seed Db with initial database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await DbInitializer.SeedAsync(dbContext);

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await DbInitializer.SeedUsersAndRolesAsync(userManager, roleManager);
}


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
