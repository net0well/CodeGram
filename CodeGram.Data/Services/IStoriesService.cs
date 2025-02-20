﻿using CodeGram.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStoriesAsync();
        Task<Story> CreateStoryAsync(Story story, IFormFile image);
    }
}
