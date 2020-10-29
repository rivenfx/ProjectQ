using Company.Project.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Riven;

using Serilog;

using System;
using System.IO;
using System.Threading.Tasks;
using Company.Project.MigratorModules;

using AppHost = Microsoft.Extensions.Hosting.Host;

namespace Company.Project.Migrator
{
    class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(args, configuration).Build();

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                  .ConfigureLogging((context, logging) => logging.ClearProviders())
                  .ConfigureLogging((context, logging) => logging.AddSerilog())
                  .ConfigureServices((services) =>
                  {
                      switch (configuration.GetDatabaseType())
                      {
                          case DatabaseType.MySql:
                              services.AddRivenModule<MySqlMigratorModule>(configuration);
                              break;
                          case DatabaseType.PostgreSQL:
                              services.AddRivenModule<PostgreSQLMigratorModule>(configuration);
                              break;
                          case DatabaseType.SqlServer:
                              services.AddRivenModule<SqlServerMigratorModule>(configuration);
                              break;
                      }

                  });
        }


        #region 日志配置

        /// <summary>
        /// 配置 SerilogLogger 配置
        /// </summary>
        /// <returns></returns>
        private static Serilog.ILogger CreateSerilogLogger()
        {
            // 获取日志配置文件
            var configuration = GetConfiguration("serilog");

            // 构建日志对象
            var cfg = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration)
                ;


            return cfg.CreateLogger();
        }

        #endregion



        #region 应用配置

        /// <summary>
        /// 获取 appsettings 配置
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration()
        {
            var configuration = GetConfiguration("appsettings");
            return configuration;
        }


        #endregion


        /// <summary>
        /// 构建配置信息
        /// </summary>
        /// <param name="fileName">文件名称,不带扩展名</param>
        /// <param name="basePath">根路径</param>
        /// <param name="environmentName">环境变量名称</param>
        /// <returns>配置信息</returns>
        public static IConfiguration GetConfiguration(string fileName, string basePath = null, string environmentName = null)
        {
            var builder = (IConfigurationBuilder)new ConfigurationBuilder();
            // 设置父级目录
            if (string.IsNullOrWhiteSpace(basePath))
            {
                var defaultBasePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
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

            // 添加环境变量
            builder = builder.AddEnvironmentVariables();

            // 构建 IConfiguration
            return builder.Build();
        }
    }
}
