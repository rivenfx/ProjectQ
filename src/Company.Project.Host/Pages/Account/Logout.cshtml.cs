using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Company.Project.Authorization.Users;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Company.Project.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager _signInManager;

        public LogoutModel(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            return RedirectToPage("Login");
        }
    }
}
