﻿using CodeGram.Controllers.Base;
using CodeGram.Data.Helpers.Constants;
using CodeGram.Data.Models;
using CodeGram.Data.Services;
using CodeGram.ViewModel.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeGram.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<User> _userManager;
        public UsersController(IUsersService usersService, UserManager<User> userManager)
        {
            _usersService = usersService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userPosts = await _usersService.GetUserPosts(userId);

            var userProfileVM = new GetUserProfileVM()
            {
                Posts = userPosts,
                User = user
            };

            return View(userProfileVM);
        }
    }
}
