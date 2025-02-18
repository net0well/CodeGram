using CodeGram.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.ViewComponents
{
    public class StoriesViewComponent : ViewComponent

    {
        private readonly AppDbContext _context;
        public StoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var allStories = await _context.Stories.Include(u => u.User).ToListAsync();

            return View(allStories);
        }

    }
}
