using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Riven.Identity.Users;

namespace Company.Project.Authorization.Users
{
    /// <summary>
    /// 系统登陆管理器
    /// </summary>
    public class SignInManager : SignInManager<User>
    {
        new UserManager UserManager => base.UserManager as UserManager;

        public SignInManager(
            UserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation
            ) : base(
                userManager,
                contextAccessor,
                claimsFactory,
                optionsAccessor,
                logger,
                schemes,
                confirmation)
        {

        }

        #region AspNetCore Identity 登陆函数

        /// <summary>
        /// 登陆 ( AspNetCore Identity )
        /// </summary>
        /// <param name="userNameOrEmailOrPhoneNumber">用户名/邮箱/手机号</param>
        /// <param name="password">密码</param>
        /// <param name="shouldLockout">需要锁定账号</param>
        /// <returns>登陆结果</returns>
        public async Task<LoginResult> LoginAsync(string userNameOrEmailOrPhoneNumber,
            string password,
            bool shouldLockout = false)
        {
            LoginResult loginResult = null;

            if (string.IsNullOrWhiteSpace(userNameOrEmailOrPhoneNumber))
            {
                throw new ArgumentNullException(nameof(userNameOrEmailOrPhoneNumber));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await this.UserManager.FindByNameOrEmailOrPhoneNumberAsync(userNameOrEmailOrPhoneNumber);


            if (user == null)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrEmailAddressOrPhoneNumber);
            }

            if (await this.UserManager.IsLockedOutAsync(user))
            {
                return new LoginResult(LoginResultType.LockedOut, user);
            }

            var verificationResult = this.UserManager.PasswordHasher
                .VerifyHashedPassword(user, user.PasswordHash, password);

            switch (verificationResult)
            {
                case PasswordVerificationResult.Success:
                    {
                        loginResult = await this.CreateLoginResultAsync(user);
                    }
                    break;
                case PasswordVerificationResult.Failed:
                    {
                        loginResult = new LoginResult(LoginResultType.InvalidPassword, user);
                    }
                    break;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    {
                        loginResult = new LoginResult(LoginResultType.PasswordNeedReset, user);
                    }
                    break;
            }

            return loginResult;
        }

        /// <summary>
        /// 创建登陆成功的结果
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task<LoginResult> CreateLoginResultAsync(User user)
        {
            if (!user.IsActive)
            {
                return new LoginResult(LoginResultType.UserIsNotActive);
            }

            var claimsPrincipal = await ClaimsFactory.CreateAsync(user);

            return new LoginResult(
                LoginResultType.Success,
                user,
                claimsPrincipal.Identity as ClaimsIdentity);
        } 

        #endregion


    }
}
