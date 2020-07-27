using Company.Project.Localization.Dtos;

using System.Collections.Generic;

namespace Company.Project.Session.Dtos
{
    /// <summary>
    /// 本地化信息
    /// </summary>
    public class LocalizationDto
    {
        /// <summary>
        /// 默认的语言
        /// </summary>
        public string DefaultCulture { get; set; }

        /// <summary>
        /// 当前语言
        /// </summary>
        public string CurrentCulture { get; set; }

        /// <summary>
        /// 所有的语言信息
        /// </summary>
        public List<LanguageInfoDto> Languages { get; set; }

    }
}
