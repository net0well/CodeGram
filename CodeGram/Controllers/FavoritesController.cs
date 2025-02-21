using CodeGram.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IPostsService _postsService;

        public FavoritesController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = 1;

            var favoritedPosts = await _postsService.GetAllFavoritedPostAsync(userId);

            return View(favoritedPosts);
        }
    }
}
