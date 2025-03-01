using CodeGram.Data;
using CodeGram.Data.Helpers.Enums;
using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Stories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.Controllers
{
    [Authorize]
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _filesService;
        public StoriesController(IStoriesService storiesService, IFilesService filesService)
        {
            _storiesService = storiesService;
            _filesService = filesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            int loggedInUserId = 1;

            var imageUploadPath = await _filesService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

            var newStory = new Story
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                ImageUrl = imageUploadPath,
                UserId = loggedInUserId
            };

            await _storiesService.CreateStoryAsync(newStory);

            return RedirectToAction("Index", "Home");
        }   
    }
}
