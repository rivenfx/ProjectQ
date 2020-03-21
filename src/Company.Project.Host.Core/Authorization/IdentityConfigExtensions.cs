using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Company.Project.Database;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;


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

            var identityBuilder = services.AddIdentity<User, Role>((options) =>
            {

            });
            identityBuilder
                .AddUserManager<UserManager>()
                .AddRoleManager<RoleManager>()
                .AddSignInManager<SignInManager>()
                
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()

                .AddUserStore<UserStore<AppDbContext>>()
                .AddRoleStore<RoleStore<AppDbContext>>()
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
            var authenticationBuilder = services
                .AddAuthentication((options) =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                });

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
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
