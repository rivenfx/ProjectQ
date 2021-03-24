using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Configuration
{
    /// <summary>
    /// 应用程序配置信息
    /// </summary>
    public class AppInfo
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Basehref { get; set; }

        public string CorsOrigins { get; set; }
    }
}
