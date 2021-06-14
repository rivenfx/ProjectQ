using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using AppHost = Microsoft.Extensions.Hosting.Host;

using Serilog;
using Serilog.Events;
using System.IO;
using Company.Project.Configuration;
using Company.Project.Debugger;

namespace Company.Project.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            try
            {
                Log.Information("Configuring ({ApplicationName})...");

                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting ({ApplicationName})...");

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

        /// <summary>
        /// 创建 HostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = GetConfiguration();

            return AppHost.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            })
                            .ConfigureAppConfiguration(x =>
                            {
                                x.AddConfiguration(configuration);
                            })
                            .ConfigureLogging((context, logging) =>
                            {
                                logging
                                  .ClearProviders()
                                  .AddSerilog();
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
            var environmentName = DebugHelper.IsDebug ? "Development" : null;
            var configuration = ConfigurationHelper.GetConfiguration(
                fileName: "appsettings",
                environmentName: environmentName
                );
            return configuration;
        }


        #endregion

    }
}
