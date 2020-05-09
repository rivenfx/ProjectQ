using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization
{
    public static class AuthenticationConfigExtenstions
    {
        /// <summary>
        /// 配置认证处理程序
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.IdentityConfiguration(configuration);

            var authenticationBuilder = services
                .AddAuthentication();

            authenticationBuilder.AddJwt(configuration);

            return authenticationBuilder;
        }

        /// <summary>
        /// 配置 AspNetCore Identity Cookie配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        static IServiceCollection IdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // 修改 asp.net core identity 默认的 cookies 认证配置
            services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, (options) =>
            {
                options.ExpireTimeSpan = new TimeSpan(0, 30, 0);
                options.SlidingExpiration = true;
            });

            return services;
        }

        /// <summary>
        /// 添加jwt认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        static AuthenticationBuilder AddJwt(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
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

            return app;
        }
    }
}
