using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Company.Project.Database;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using System;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authentication.Cookies;

using Riven;
using Riven.Extensions;
using Riven.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Company.Project.Authorization
{
    public static class IdentityConfigExtensions
    {
        /// <summary>
        /// 注册配置Identiy基础服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IdentityBuilder IdentityRegister(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, UserPasswordHasher>();
            services.AddTransient<UserPasswordHasher>();

            // 添加 Identity
            var identityBuilder = services.AddIdentity<User, Role>((options) =>
            {
                options.ConfigurationClaimsIdentity();
                options.ConfigurationLockout();
                options.ConfigurationPassword();
                options.ConfigurationSignIn();
                options.ConfigurationToken();
                options.ConfigurationUser();
            });
            identityBuilder
                .AddUserManager<UserManager>()
                .AddRoleManager<RoleManager>()
                .AddUserStore<UserStore<AppDbContext>>()
                .AddRoleStore<RoleStore<AppDbContext>>()
                .AddSignInManager<SignInManager>()
                .AddClaimsPrincipalFactory<IdentityUserClaimsPrincipalFactory<User, Role>>()
                .AddDefaultTokenProviders();

            // 添加 Riven.Identity ClaimAccessor
            services.AddRivenIdentityClaimAccesssor<RoleManager, UserManager>();

            // 添加 Claims 授权方式
            services.AddRivenAspNetCoreClaimsAuthorization();

            return identityBuilder;
        }

      

      



        #region 私有 IdentityOptions 配置函数

        /// <summary>
        /// 密码 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationPassword(this IdentityOptions options)
        {
            options.Password.RequiredLength = 6; // 密码的最小长度
            options.Password.RequireDigit = true; // 要求密码中的数字介于0-9 之间
            options.Password.RequireLowercase = false; // 要求小写字母
            options.Password.RequireUppercase = false; // 要求大写字母
            options.Password.RequireNonAlphanumeric = false; // 密码中需要一个非字母数字字符
            options.Password.RequiredUniqueChars = 1;  // 需要密码中的非重复字符数。

            return options;
        }


        /// <summary>
        /// 登录 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationSignIn(this IdentityOptions options)
        {
            options.SignIn.RequireConfirmedEmail = false; // 需要验证邮箱
            options.SignIn.RequireConfirmedPhoneNumber = false; // 需要验证手机号码
            options.SignIn.RequireConfirmedAccount = false; // 需要确认账户

            return options;
        }

        /// <summary>
        /// 用户 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationUser(this IdentityOptions options)
        {
            options.User.RequireUniqueEmail = true; // 用户邮箱必须唯一

            return options;
        }

        /// <summary>
        /// Token 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationToken(this IdentityOptions options)
        {
            //options.Tokens

            return options;
        }

        /// <summary>
        /// ClaimsIdentity 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationClaimsIdentity(this IdentityOptions options)
        {
            options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
            return options;
        }

        /// <summary>
        /// Lockout 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IdentityOptions ConfigurationLockout(this IdentityOptions options)
        {
            options.Lockout.MaxFailedAccessAttempts = 5; // 允许错误登录次数
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);// 锁定时间 3 分钟
            options.Lockout.AllowedForNewUsers = true; // 允许注册新用户


            return options;
        }

        #endregion
    }
}
