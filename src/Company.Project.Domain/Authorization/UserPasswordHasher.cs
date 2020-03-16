using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization
{
    /// <summary>
    /// 用户密码处理器
    /// </summary>
    public class UserPasswordHasher : PasswordHasher<User>
    {
        public override string HashPassword(User user, string password)
        {
            return base.HashPassword(user, password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
