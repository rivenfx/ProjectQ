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
        /// 根据用户id直接创建登录成功结果
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginByUserIdAsync(string userId)
        {

            var user = await this.UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrEmailAddressOrPhoneNumber);
            }

            if (await this.UserManager.IsLockedOutAsync(user))
            {
                return new LoginResult(LoginResultType.LockedOut, user);
            }

            return await this.CreateLoginResultAsync(user);
        }

        /// <summary>
        /// 根据用户名/邮箱/手机号直接创建登录成功结果
        /// </summary>
        /// <param name="userNameOrEmailOrPhoneNumber"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginByUserNameOrEmailOrPhoneNumberAsync(string userNameOrEmailOrPhoneNumber)
        {

            var user = await this.UserManager.FindByNameOrEmailOrPhoneNumberAsync(userNameOrEmailOrPhoneNumber);

            if (user == null)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrEmailAddressOrPhoneNumber);
            }

            if (await this.UserManager.IsLockedOutAsync(user))
            {
                return new LoginResult(LoginResultType.LockedOut, user);
            }

            return await this.CreateLoginResultAsync(user);
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

            return new LoginResult(LoginResultType.Success, user, claimsPrincipal);
        }

        #endregion

        /// <summary>
        /// Signs in the specified <paramref name="userPrincipal"/>.
        /// </summary>
        /// <param name="userPrincipal">The userPrincipal to sign-in.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task SignInWithClaimsIdentityAsync(ClaimsPrincipal userPrincipal, bool isPersistent, string authenticationMethod = null)
        {
            return this.SignInWithClaimsIdentityAsync(
                userPrincipal,
                new AuthenticationProperties() { IsPersistent = isPersistent },
                authenticationMethod
                );
        }

        /// <summary>
        /// Signs in the specified <paramref name="userPrincipal"/>.
        /// </summary>
        /// <param name="userPrincipal">The userPrincipal to sign-in.</param>
        /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task SignInWithClaimsIdentityAsync(ClaimsPrincipal userPrincipal, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            await Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }


    }
}
