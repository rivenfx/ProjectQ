using System;
using System.Collections.Generic;
using System.Text;

using Riven.Dependency;
using Riven.Localization;

namespace Company.Project.Authorization
{
    public interface IAppSession
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// 用户ID - 字符串类型
        /// </summary>
        string UserIdString { get; }

        /// <summary>
        /// 用户账号
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 当前语言信息
        /// </summary>
        LanguageInfo CurrentLanguage { get; }
    }
}
