using CodeGram.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
    }
}
