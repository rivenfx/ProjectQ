using Microsoft.Extensions.DependencyInjection;

using System;
using Company.Project.Authorization.Permissions;

namespace Company.Project
{
    /// <summary>
    /// 应用权限常量
    /// </summary>
    public static class AppPermissions
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
        /// 租户
        /// </summary>
        public static class Tenant
        {
            public const string Node = "tenant.node";
            public const string Query = "tenant.query";
            public const string Create = "tenant.create";
            public const string Edit = "tenant.edit";
            public const string Delete = "tenant.delete";
        }
    }

}
