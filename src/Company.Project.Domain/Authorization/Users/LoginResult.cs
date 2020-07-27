using Riven.Identity.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class LoginResult : LoginResult<User>
    {
        public LoginResult(LoginResultType result, User user = null, ClaimsPrincipal claimsPrincipal = null)
            : base(result, user, claimsPrincipal)
        {

        }

    }
}
