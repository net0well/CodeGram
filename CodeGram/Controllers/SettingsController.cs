using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
