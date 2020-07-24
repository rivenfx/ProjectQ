using System;
using System.Collections.Generic;
using System.Text;

using Riven.Dependency;
using Riven.Localization;

namespace Company.Project.Authorization
{
    public interface IAppSession : ITransientDependency
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// 用户账号
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 租户Id - 字符串类型
        /// </summary>
        string TenantName { get; }

        /// <summary>
        /// 模拟登录用户Id
        /// </summary>
        Guid? ImpersonatedUserId { get; }

        /// <summary>
        /// 模拟登录租户名称 - 字符串
        /// </summary>
        string ImpersonatedTenantName { get; }

        /// <summary>
        /// 当前语言信息
        /// </summary>
        LanguageInfo CurrentLanguage { get; }
    }
}
