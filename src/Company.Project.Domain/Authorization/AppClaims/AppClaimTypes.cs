using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    /// <summary>
    /// 应用的Claims类型键值
    /// </summary>
    public static class AppClaimTypes
    {
        /// <summary>
        /// 租户Id键
        /// </summary>
        public const string TenantIdNameIdentifier = "RivenFx.TenantIdNameIdentifier";

        /// <summary>
        /// 模拟登录用户id 键
        /// </summary>
        public const string ImpersonatedUserIdNameIdentifier = "RivenFx.ImpersonatedUserIdNameIdentifier";

        /// <summary>
        /// 模拟登录租户id 键
        /// </summary>
        public const string ImpersonatedTenantIdNameIdentifier = "RivenFx.ImpersonatedTenantIdNameIdentifier";


    }
}
