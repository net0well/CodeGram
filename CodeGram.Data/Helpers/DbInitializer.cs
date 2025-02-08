using CodeGram.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Helpers
{
    public static class DbInitializer
    {
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
