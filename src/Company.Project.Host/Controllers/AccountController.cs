using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Company.Project.Models;
using Microsoft.AspNetCore.Authorization;
using Company.Project.Authenticate.Dtos;
using Company.Project.Authorization.Users;

namespace Company.Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager _signInManager;

        public AccountController(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.IsEnabledMultiTenancy = Riven.MultiTenancy.MultiTenancyConfig.IsEnabled;

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
