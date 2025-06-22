using CodeGram.Data.Helpers.Constants;
using CodeGram.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //roles
            if(!roleManager.Roles.Any())
            {
                foreach(var roleName in AppRoles.All)
                {
                    if(!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    }
                }
            }

            //users with roles

            if(!userManager.Users.Any(n => !string.IsNullOrEmpty(n.Email)))
            {
                var userPassword = "SenhaTop@123";
                var newUsers = new User()
                {
                    UserName = "net0well",
                    Email = "wellingtonalneto@gmail.com",
                    FullName = "Wellington Neto",
                    ProfilePictureUrl = "https://media.licdn.com/dms/image/v2/D4D03AQHHaU8CD8LE4A/profile-displayphoto-shrink_800_800/profile-displayphoto-shrink_800_800/0/1711635684506?e=1744243200&v=beta&t=Qs3bY7QBtEqU-U-WMTZvcs7rcI1JA-fuK9TbbyFzd7E",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUsers, userPassword);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUsers, AppRoles.User);
                }

                var newAdmin = new User()
                {
                    UserName = "admin.admin",
                    Email = "wellingtonadm@gmail.com",
                    FullName = "Admin",
                    ProfilePictureUrl = "https://media.licdn.com/dms/image/v2/D4D03AQHHaU8CD8LE4A/profile-displayphoto-shrink_800_800/profile-displayphoto-shrink_800_800/0/1711635684506?e=1744243200&v=beta&t=Qs3bY7QBtEqU-U-WMTZvcs7rcI1JA-fuK9TbbyFzd7E",
                    EmailConfirmed = true
                };

                var resultNewAdmin = await userManager.CreateAsync(newAdmin, userPassword);

                if (resultNewAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, AppRoles.Admin);
                }
            }
        }

        public static async Task SeedAsync(AppDbContext appDbContext)
        {
            if (!appDbContext.Users.Any() && !appDbContext.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Wellington Neto",
                    ProfilePictureUrl = "https://media.licdn.com/dms/image/v2/D4D03AQHHaU8CD8LE4A/profile-displayphoto-shrink_800_800/profile-displayphoto-shrink_800_800/0/1711635684506?e=1744243200&v=beta&t=Qs3bY7QBtEqU-U-WMTZvcs7rcI1JA-fuK9TbbyFzd7E"
                };
                await appDbContext.Users.AddAsync(newUser);
                await appDbContext.SaveChangesAsync();

                var newPostWithoutImage = new Post()
                {
                    Content = "Why C# is better than Java?",
                    ImageUrl = "",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                var newPostWithImage = new Post()
                {
                    Content = "Why crazy people use JavaScript on back-end projects?",
                    ImageUrl = "https://www.naveedulhaq.com/wp-content/uploads/2023/06/asp.net_-1024x576.png",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                await appDbContext.Posts.AddRangeAsync(newPostWithoutImage, newPostWithImage);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
