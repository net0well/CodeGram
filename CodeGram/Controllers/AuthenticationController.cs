using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class AuthenticationController : Controller
    {
        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();  
        }
    }
}
