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
using System.Threading.Tasks;
using JetBrains.Annotations;
using Riven;
using Microsoft.AspNetCore.Authentication.Cookies;
using Riven.Uow;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Company.Project.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Riven.Identity.Authorization;

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

            // 使用 Riven 的扩展函数添加 Identity
            var identityBuilder = services.AddRivenIdentity<User, Role, UserManager, RoleManager, UserStore<AppDbContext>, RoleStore<AppDbContext>, SignInManager>((options) =>
             {
                 options.ConfigurationUser()
                     .ConfigurationPassword()
                     .ConfigurationSignIn()
                     .ConfigurationLockout()
                     .ConfigurationToken()
                     .ConfigurationClaimsIdentity();
             });

            identityBuilder
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();

            return identityBuilder;
        }

        /// <summary>
        /// 配置认证处理程序
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder IdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, RoleClaimAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, RoleClaimAuthorizationRequirement>();

            var authenticationBuilder = services
                .AddAuthentication();

            #region 配置 Identity 默认自带的 cookie 校验器

            // 自定义的校验器
            services.TryAddScoped<CookieSecurityStampValidator>();
            // 配置
            services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, (options) =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnValidatePrincipal = async (context) =>
                    {
                        var validator = context.HttpContext.RequestServices.GetService<CookieSecurityStampValidator>();
                        await validator.ValidateAsync(context);
                    }
                };
            });

            #endregion

            authenticationBuilder.AddJwt(configuration);

            return authenticationBuilder;
        }

        /// <summary>
        /// 添加jwt认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwt(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {
            authenticationBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (options) =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = configuration["Authentication:JwtBearer:Audience"];
                options.Authority = configuration["Authentication:JwtBearer:Authority"];
            });

            return authenticationBuilder;
        }

        /// <summary>
        /// 启用系统的Authentication和Authorization
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppAuthenticationAndAuthorization(this IApplicationBuilder app)
        {
            app.UseDefaultAuthentication();

            app.UseJwtAuthentication();

            app.UseAuthorization();

            //app.UseCookiePolicy(new CookiePolicyOptions()
            //{
            //    CheckConsentNeeded = context => true,
            //    MinimumSameSitePolicy = SameSiteMode.None,
            //});

            return app;
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
            //options.ClaimsIdentity

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
