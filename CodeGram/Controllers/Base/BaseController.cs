﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeGram.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected int? GetUserId()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return null;
            }
            return int.Parse(loggedInUserId);
        }

        protected string GetUserFullName()
        {
            var loggedInUserFullName = User.FindFirstValue(ClaimTypes.Name);
            //var GivenName = User.FindFirstValue(ClaimTypes.GivenName);
            //var Surname = User.FindFirstValue(ClaimTypes.Surname);

            return loggedInUserFullName;
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Authentication");
        }
    }
}
