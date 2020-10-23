using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    /// <summary>
    /// 应用的Claims类型键值
    /// </summary>
    public static class AppClaimsTypes
    {
        /// <summary>
        /// 租户名称 键
        /// </summary>
        public const string TenantNameIdentifier = "RivenFx.TenantIdNameIdentifier";

        /// <summary>
        /// 模拟登录用户id 键
        /// </summary>
        public const string ImpersonatedUserIdNameIdentifier = "RivenFx.ImpersonatedUserIdNameIdentifier";

        /// <summary>
        /// 模拟登录租户名称 键
        /// </summary>
        public const string ImpersonatedTenantNameIdentifier = "RivenFx.ImpersonatedTenantIdNameIdentifier";


    }
}
