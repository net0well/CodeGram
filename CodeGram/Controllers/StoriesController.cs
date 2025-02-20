using CodeGram.Data;
using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        public StoriesController(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            int loggedInUserId = 1;

            var newStory = new Story
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

            await _storiesService.CreateStoryAsync(newStory, storyVM.Image);

            return RedirectToAction("Index", "Home");
        }
    }
}
