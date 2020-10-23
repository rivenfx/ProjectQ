using Company.Project.Authorization.AppClaims;

using Microsoft.Extensions.DependencyInjection;

using Riven.Application;
using Riven.Modular;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using Riven.Identity.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Project
{
    /// <summary>
    /// 应用权限常量
    /// </summary>
    public static class AppClaimsConsts
    {
        public const string RootNode = "root";

        /// <summary>
        /// 用户
        /// </summary>
        public static class User
        {
            public const string Node = "user.node";
            public const string Query = "user.query";
            public const string Create = "user.create";
            public const string Edit = "user.edit";
            public const string Delete = "user.delete";
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static class Role
        {
            public const string Node = "role.node";
            public const string Query = "role.query";
            public const string Create = "role.create";
            public const string Edit = "role.edit";
            public const string Delete = "role.delete";
        }


        /// <summary>
        /// 注册 claims
        /// </summary>
        /// <param name="serviceProvider">注入容器</param>
        /// <returns></returns>
        public static IServiceProvider RegisterBasicClaims(this IServiceProvider serviceProvider)
        {
            var claimsManager = serviceProvider.GetRequiredService<IClaimsManager>();
            claimsManager.Init();
            return serviceProvider;
        }
    }

}
