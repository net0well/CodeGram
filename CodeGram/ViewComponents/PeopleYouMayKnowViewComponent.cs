using CodeGram.Data.Services;
using CodeGram.ViewModel.Friends;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeGram.ViewComponents
{
    public class PeopleYouMayKnowViewComponent : ViewComponent
    {
        private readonly IFriendsService _friendsService;
        public PeopleYouMayKnowViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(loggedInUserId);

            var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(userId);
            var suggestedFriendsVM = suggestedFriends.Select(n => new UserWithFriendsCountDtoVM
            {
                UserId = n.User.Id,
                FullName = n.User.FullName,
                ProfilePictureUrl = n.User.ProfilePictureUrl,
                FriendsCount = n.FriendsCount
            }).ToList();

            return View(suggestedFriendsVM);
        }
    }
}
