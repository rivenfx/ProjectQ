using Riven.Identity.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class LoginResult : LoginResult<User>
    {
        public LoginResult(LoginResultType result, User user = null, ClaimsIdentity identity = null)
            : base(result, user, identity)
        {

        }

    }
}
