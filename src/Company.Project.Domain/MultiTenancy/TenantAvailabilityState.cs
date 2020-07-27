using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy
{
    /// <summary>
    /// 租户可用性状态
    /// </summary>
    public enum TenantAvailabilityState
    {
        /// <summary>
        /// 已激活
        /// </summary>
        Available = 1,
        /// <summary>
        /// 未激活
        /// </summary>
        InActive = 2,
        /// <summary>
        /// 未找到
        /// </summary>
        NotFound = 3,
    }
}
