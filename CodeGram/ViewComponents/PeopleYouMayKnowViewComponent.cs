using Microsoft.AspNetCore.Mvc;

namespace CodeGram.ViewComponents
{
    public class PeopleYouMayKnowViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
