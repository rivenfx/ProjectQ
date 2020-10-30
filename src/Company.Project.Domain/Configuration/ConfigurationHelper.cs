using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Company.Project.Configuration
{
    /// <summary>
    /// 配置帮助器
    /// </summary>
    public static class ConfigurationHelper
    {
        static IDictionary<string, IConfiguration> _configurationCache;

        static ConfigurationHelper()
        {
            _configurationCache = new Dictionary<string, IConfiguration>();
        }

        /// <summary>
        /// 构建配置信息
        /// </summary>
        /// <param name="fileName">文件名称,不带扩展名</param>
        /// <param name="basePath">根路径</param>
        /// <param name="environmentName">环境变量名称</param>
        /// <param name="configurationBuilderAction">自定义配置流程</param>
        /// <param name="cache">启用缓存,默认为true</param>
        /// <returns>配置信息</returns>
        public static IConfiguration GetConfiguration(
            string fileName,
            string basePath = null,
            string environmentName = null,
            Func<IConfigurationBuilder, IConfigurationBuilder> configurationBuilderAction = null,
            bool cache = true
            )
        {
            // 从缓存获取
            var cacheKey = $"{basePath ?? ""}#{fileName}#{environmentName ?? ""}";
            if (cache && _configurationCache.TryGetValue(cacheKey, out IConfiguration configuration))
            {
                return configuration;
            }


            var builder = (IConfigurationBuilder)new ConfigurationBuilder();
            // 设置父级目录
            if (string.IsNullOrWhiteSpace(basePath))
            {
                var defaultBasePath = Path.GetDirectoryName(typeof(ConfigurationHelper).Assembly.Location);
                builder = builder.SetBasePath(defaultBasePath);
            }
            else
            {
                builder = builder.SetBasePath(basePath);
            }

            // 添加基本文件
            builder = builder
                .AddJsonFile($"{fileName}.json", optional: false, reloadOnChange: true);

            // 添加带环境的文件
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder
                    .AddJsonFile(
                        $"{fileName}.{environmentName}.json",
                        optional: true,
                        reloadOnChange: true
                    );
            }

            // 自定义
            if (configurationBuilderAction != null)
            {
                builder = configurationBuilderAction?.Invoke(builder);
            }

            // 添加环境变量
            builder = builder.AddEnvironmentVariables();

            // 构建 IConfiguration
            configuration = builder.Build();

            // 缓存
            if (cache)
            {
                _configurationCache[cacheKey] = configuration;
            }

            return configuration;
        }
    }
}
