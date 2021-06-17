using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Company.Project.Authorization.Users;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Company.Project.Pages.Account
{
    public class LogoutModel : PageModel
    {
        readonly SignInManager _signInManager;
        readonly CookieAuthenticationOptions _cookieAuthenticationOptions;

        public LogoutModel(SignInManager signInManager,
             IOptionsSnapshot<CookieAuthenticationOptions> cookieAuthenticationOptions
            )
        {
            this._signInManager = signInManager;
            this._cookieAuthenticationOptions = cookieAuthenticationOptions.Get(IdentityConstants.ApplicationScheme);
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            return Redirect(_cookieAuthenticationOptions.LoginPath);
        }
    }
}
