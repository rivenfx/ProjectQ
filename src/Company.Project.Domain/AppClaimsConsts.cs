using Company.Project.Authorization.AppClaims;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{
    /// <summary>
    /// 应用权限常量
    /// </summary>
    public static class AppClaimsConsts
    {
        /// <summary>
        /// 用户
        /// </summary>
        public static class User
        {
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
            claimsManager.Add(CommonClaims());
            claimsManager.Add(HostClaims());
            claimsManager.Add(TenantClaims());

            return serviceProvider;
        }

        private static ClaimItem[] CommonClaims()
        {
            return new ClaimItem[]
            {
                ClaimItem.CreateWithCommon(AppClaimsConsts.User.Query),
                ClaimItem.CreateWithCommon(AppClaimsConsts.User.Create),
                ClaimItem.CreateWithCommon(AppClaimsConsts.User.Edit),
                ClaimItem.CreateWithCommon(AppClaimsConsts.User.Delete),

                ClaimItem.CreateWithCommon(AppClaimsConsts.Role.Query),
                ClaimItem.CreateWithCommon(AppClaimsConsts.Role.Create),
                ClaimItem.CreateWithCommon(AppClaimsConsts.Role.Edit),
                ClaimItem.CreateWithCommon(AppClaimsConsts.Role.Delete),
            };
        }

        private static ClaimItem[] HostClaims()
        {
            return new ClaimItem[]
            {

            };
        }

        private static ClaimItem[] TenantClaims()
        {
            return new ClaimItem[]
            {

            };
        }



    }
}
