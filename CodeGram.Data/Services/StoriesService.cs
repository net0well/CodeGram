﻿using CodeGram.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly AppDbContext _context;
        public StoriesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Story>> GetAllStoriesAsync()
        {
            var allStories = await _context.Stories
                .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24))
                .Include(u => u.User)
                .ToListAsync();

            return allStories;
        }

        public async Task<Story> CreateStoryAsync(Story story)
        {
            

            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            return story;
        }
    }
}
