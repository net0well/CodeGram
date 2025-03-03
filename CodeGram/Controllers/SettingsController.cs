﻿using CodeGram.Data.Helpers.Enums;
using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeGram.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;
        public SettingsController(IUsersService usersService,
            IFilesService filesService,
            UserManager<User> userManager)
        {
            _usersService = usersService;
            _filesService = filesService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDb = await _usersService.GetUser(int.Parse(loggedInUserId));

            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(loggedInUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureVM profilePictureVM)
        {
            var loggedInUser = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, Data.Helpers.Enums.ImageFileType.ProfilePicture);

            await _usersService.UpdateUserProfilePicture(loggedInUser, uploadedProfilePictureUrl);

            return RedirectToAction("Index");
        }
    }
}
