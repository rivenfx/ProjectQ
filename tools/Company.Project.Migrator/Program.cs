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
        static int Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationName})...");

                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting web host ({ApplicationName})...");
                
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationName})!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = GetConfiguration();

            return AppHost.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration(x =>
                  {
                      x.AddConfiguration(configuration);
                  })
                  .ConfigureLogging((context, logging) =>
                  {
                      logging
                        .ClearProviders()
                        .AddSerilog();
                  })
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
            var configuration = ConfigurationHelper.GetConfiguration("serilog");

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
            var configuration = ConfigurationHelper.GetConfiguration("appsettings");
            return configuration;
        }


        #endregion
    }
}
