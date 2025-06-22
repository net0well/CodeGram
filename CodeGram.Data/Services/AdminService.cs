using CodeGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetReportedPostsAsync()
        {
            var posts = await _context.Posts
                .Include(n => n.User)
                .Include(n => n.Comments)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Where(n => n.NrOfReports > 5 && !n.IsDeleted)
                .ToListAsync();

            return posts;
        }
        public async Task ApproveReport(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);
            if (postDb != null)
            {
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectReport(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if(postDb != null)
            {
                postDb.NrOfReports = 0;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();  
            }
        }
    }
}
