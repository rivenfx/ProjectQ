using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{
    public static class AppClaimsConsts
    {
        public static class User
        {
            public const string Create = "user.create";
            public const string Edit = "user.edit";
            public const string Delete = "user.delete";
        }

        public static class Role
        {
            public const string Create = "role.create";
            public const string Edit = "role.edit";
            public const string Delete = "role.delete";
        }
    }
}
