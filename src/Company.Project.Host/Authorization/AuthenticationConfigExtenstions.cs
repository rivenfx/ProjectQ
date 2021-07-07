using AspNetCore.Authentication.ApiToken;

using Company.Project.Authorization.Tokens;
using Company.Project.Configuration;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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

            authenticationBuilder
                .AddJwt(configuration)
                .AddApiToken()
                ;

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
            IdentityModelEventSource.ShowPII = true; //Add this line

            authenticationBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (options) =>
            {
                var jwtBearerInfo = configuration.GetJwtBearerInfo();

                options.RequireHttpsMetadata = false;
                options.Audience = jwtBearerInfo.Audience;
                options.Authority = jwtBearerInfo.Authority;
                options.ClaimsIssuer = jwtBearerInfo.Issuer;

                options.TokenValidationParameters = new TokenValidationParameters
                {


                    // 签名键必须匹配!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtBearerInfo.SecurityKey)),

                    // 验证JWT发行者(iss)的 claim
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerInfo.Issuer,

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = jwtBearerInfo.Audience,

                    // 必须指定过期时间
                    RequireExpirationTime = true,

                    // 验证过期
                    ValidateLifetime = true,

                    // 时间偏移
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            return authenticationBuilder;
        }

        /// <summary>
        /// 添加 Api Token 认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <returns></returns>
        static AuthenticationBuilder AddApiToken(this AuthenticationBuilder authenticationBuilder)
        {
            authenticationBuilder.AddApiToken(option =>
                {
                    option.UseCache = false;
                })
                .AddProfileService<AppTokenProfileService>()
                .AddTokenStore<AppTokenStore>()
                .AddCleanService();
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
