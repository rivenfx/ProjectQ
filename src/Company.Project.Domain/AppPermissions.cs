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
        /// 注册 Permission
        /// </summary>
        /// <param name="serviceProvider">注入容器</param>
        /// <returns></returns>
        public static IServiceProvider RegisterPermissions(this IServiceProvider serviceProvider)
        {
            var permissionManager = serviceProvider.GetRequiredService<IPermissionManager>();
            permissionManager.Init();
            return serviceProvider;
        }
    }

}
