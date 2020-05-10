using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Localization.Dtos
{
    public class LanguageInfoDto
    {
        /// <summary>
        /// Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// 语言显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 本地化的数据
        /// </summary>
        public Dictionary<string, string> Texts { get; set; }
    }
}
