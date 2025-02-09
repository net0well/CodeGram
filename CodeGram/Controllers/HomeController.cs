using CodeGram.Data;
using CodeGram.Data.Models;
using CodeGram.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeGram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var allPosts = await _context.Posts
                .Include(n => n.User)
                .ToListAsync();
            return View(allPosts);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user

            int loggedInUser = 1;

            //Create a new post

            var newPost = new Post
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = "",
                NrOfReports = 0,
                UserId = loggedInUser
            };

            //add post to the db

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

    }
}
