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
using Riven.MultiTenancy;

namespace Company.Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager _signInManager;
        private readonly IMultiTenancyOptions _multiTenancyOptions;

        public AccountController(SignInManager signInManager, IMultiTenancyOptions multiTenancyOptions)
        {
            _signInManager = signInManager;
            _multiTenancyOptions = multiTenancyOptions;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.IsEnabledMultiTenancy = _multiTenancyOptions.IsEnabled;

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

            return Redirect("../Account/Login");
        }
    }
}
