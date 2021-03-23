using System;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Riven;
using Riven.Authorization;

using Company.Project.Database;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Authorization.Permissions;
using Riven.Identity;
using Company.Project.Identity;

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
            // 用户密码加密器
            services.AddScoped<IPasswordHasher<User>, UserPasswordHasher>();

            // 添加 Identity
            var identityBuilder = services.AddRivenIdentity<User, Role, Permission>((options) =>
            {
                options.ConfigurationClaimsIdentity();
                options.ConfigurationLockout();
                options.ConfigurationPassword();
                options.ConfigurationSignIn();
                options.ConfigurationToken();
                options.ConfigurationUser();
            });
            identityBuilder
                // 用户管理器
                .AddUserManager<UserManager>()
                // 角色管理器
                .AddRoleManager<RoleManager>()
                // 权限管理器
                .AddPermissionManager<PermissionManager>()
                // 登录管理器
                .AddSignInManager<SignInManager>()
                // 用户存储器
                .AddUserStore<UserStore>()
                // 角色存储器
                .AddRoleStore<RoleStore>()
                // 权限存储器
                .AddPermissionStore<PermissionStore>()
                // 权限初始化器
                .AddPermissionInitializer<ModulerPermissionInitializer>()
                // 用户角色查找器
                .AddUserRoleFinder<UserStore>()
                // ClaimsPrincipal 创建器
                .AddClaimsPrincipalFactory<IdentityUserClaimsPrincipalFactory<User, Role, Guid>>()
                // 添加 DbContext 访问器
                .AddDbContextAccessor<UowIDbContextAccessor>()
                // token提供者
                .AddDefaultTokenProviders()
                // 使用 Riven 实现的权限校验
                .AddRivenAspNetCorePermissionAuthorization()
                ;

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
