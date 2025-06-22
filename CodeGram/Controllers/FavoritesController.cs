using CodeGram.Data.Helpers.Constants;
using CodeGram.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeGram.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class FavoritesController : Controller
    {
        private readonly IPostsService _postsService;

        public FavoritesController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoritedPosts = await _postsService.GetAllFavoritedPostAsync(int.Parse(userId));

            return View(favoritedPosts);
        }
    }
}
