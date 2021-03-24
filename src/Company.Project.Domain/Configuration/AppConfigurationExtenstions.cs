using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Configuration
{
    public static class AppConfigurationExtenstions
    {
        public static class AppConfigurationConsts
        {
            public const string DatabaseType = "ConnectionStrings:DatabaseType";
            public const string DefaultDatabaseConnectionString = "ConnectionStrings:Default";
            public const string AppName = "App:Name";
            public const string AppVersion = "App:Version";
            public const string AppBasehref = "App:Basehref";
            public const string AppCorsOrigins = "App:CorsOrigins";

            public const string MultiTenancyIsEnabled = "MultiTenancy:IsEnabled";



            public const string AuthenticationJwtBearerAudience = "Authentication:JwtBearer:Audience";
            public const string AuthenticationJwtBearerAuthority = "Authentication:JwtBearer:Authority";
            public const string AuthenticationJwtBearerIssuer = "Authentication:JwtBearer:Issuer";
            public const string AuthenticationJwtBearerSecurityKey = "Authentication:JwtBearer:SecurityKey";

        }


        /// <summary>
        /// 获取默认数据库类型
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DatabaseType GetDatabaseType(this IConfiguration configuration)
        {
            switch (configuration[AppConfigurationConsts.DatabaseType]?.Trim().ToLowerInvariant())
            {
                case "mysql":
                    return DatabaseType.MySql;
                case "postgresql":
                    return DatabaseType.PostgreSQL;
                case "sqlserver":
                default:
                    return DatabaseType.SqlServer;
            }
        }

        /// <summary>
        /// 获取默认数据库连接字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetDefaultDatabaseConnectionString(this IConfiguration configuration)
        {
            return configuration[AppConfigurationConsts.DefaultDatabaseConnectionString];
        }

        /// <summary>
        /// 获取应用信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AppInfo GetAppInfo(this IConfiguration configuration)
        {
            return new AppInfo()
            {
                Name = configuration[AppConfigurationConsts.AppName],
                Version = configuration[AppConfigurationConsts.AppVersion],
                Basehref = configuration[AppConfigurationConsts.AppBasehref],
                CorsOrigins = configuration[AppConfigurationConsts.AppCorsOrigins],
            };
        }

        /// <summary>
        /// 获取认证配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationInfo GetAuthenticationInfo(this IConfiguration configuration)
        {
            return new AuthenticationInfo()
            {
                JwtBearer = configuration.GetJwtBearerInfo()
            };
        }

        /// <summary>
        /// 获取JwtBearer配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static JwtBearerInfo GetJwtBearerInfo(this IConfiguration configuration)
        {
            return new JwtBearerInfo(
                configuration[AppConfigurationConsts.AuthenticationJwtBearerAudience],
                configuration[AppConfigurationConsts.AuthenticationJwtBearerAuthority],
                configuration[AppConfigurationConsts.AuthenticationJwtBearerIssuer],
                configuration[AppConfigurationConsts.AuthenticationJwtBearerSecurityKey]
                );
        }

        /// <summary>
        /// 获取多租户配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static MultiTenancyInfo GetMultiTenancyInfo(this IConfiguration configuration)
        {
            return new MultiTenancyInfo()
            {
                IsEnabled = bool.Parse(configuration[AppConfigurationConsts.MultiTenancyIsEnabled])
            };
        }

    }
}
