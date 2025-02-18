using CodeGram.Data;
using Microsoft.AspNetCore.Mvc;

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

            return View();

        }

    }
}
