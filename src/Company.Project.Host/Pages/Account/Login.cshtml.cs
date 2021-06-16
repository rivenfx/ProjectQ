using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Company.Project.Authorization.Users;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Riven.MultiTenancy;

namespace Company.Project.Pages.Account
{
    public class LoginModel : PageModel
    {
        readonly IMultiTenancyOptions _multiTenancyOptions;

        public LoginModel(IMultiTenancyOptions multiTenancyOptions)
        {
            _multiTenancyOptions = multiTenancyOptions;
        }


        public bool IsEnabledMultiTenancy { get; protected set; }


        public void OnGet()
        {
            this.IsEnabledMultiTenancy = this._multiTenancyOptions.IsEnabled;
        }
    }
}
