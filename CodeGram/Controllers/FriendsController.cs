using CodeGram.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class FriendsController : Controller
    {
        public readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
