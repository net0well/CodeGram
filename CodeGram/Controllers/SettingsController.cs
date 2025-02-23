using CodeGram.Data.Helpers.Enums;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Settings;
using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        public SettingsController(IUsersService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;

            var userDb = await _usersService.GetUser(loggedInUserId);

            return View(userDb);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureVM profilePictureVM)
        {
            var loggedInUserId = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfilePicture);

            await _usersService.UpdateUserProfilePicture(loggedInUserId, uploadedProfilePictureUrl);

            return RedirectToAction("Index");
        }
    }
}
