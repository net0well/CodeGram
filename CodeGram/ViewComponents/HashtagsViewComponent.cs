using CodeGram.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeGram.ViewComponents
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HashtagsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oneWeekAgoNow = DateTime.UtcNow.AddDays(-7);

            var topFiveHashtags = await _context.Hashtags
                .Where(h => h.DateCreated >= oneWeekAgoNow)
                .OrderByDescending(n => n.Count)
                .Take(5)
                .ToListAsync();


            return View(topFiveHashtags);

        }

    }
}
