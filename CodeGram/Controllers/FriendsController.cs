using CodeGram.Controllers.Base;
using CodeGram.Data.Helpers.Constants;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Friends;
using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class FriendsController : BaseController
    {
        public readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var friendsData = new FriendshipVM
            {
                FriendRequestsSent = await _friendsService.GetSentFriendRequestAsync(userId.Value),
                FriendRequestsReceived = await _friendsService.GetReceivedFriendRequestAsync(userId.Value)
            };

            return View(friendsData);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CancelFriendRequest(int requestId)
        {
           await _friendsService.UpdateRequestAsync(requestId, FriendshipStatus.Canceled);
           return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            await _friendsService.UpdateRequestAsync(requestId, FriendshipStatus.Accepted);
            return RedirectToAction("Index");
        }
    }
}
