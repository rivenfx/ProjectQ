using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization
{
    /// <summary>
    /// asp.net core identity ClaimType const
    /// </summary>
    public static class IdentityClaimTypes
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
