
using Riven.Localization;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Session.Dtos
{
    /// <summary>
    /// 登录会话信息
    /// </summary>
    public class SessionDto
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 应用版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 当前登录用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public ClaimsDto Auth { get; set; }

        /// <summary>
        /// 本地化
        /// </summary>
        public LocalizationDto Localization { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public string Menu { get; set; }

    }
}
