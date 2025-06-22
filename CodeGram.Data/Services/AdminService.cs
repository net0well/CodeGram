﻿using CodeGram.Data.Models;
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
                .Include(n => n.Reports)
                .Where(n => n.Reports.Count > 5 && !n.IsDeleted)
                .ToListAsync();

            return posts;
        }
    }
}
