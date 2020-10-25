using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Riven;

using Serilog;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Company.Project.Migrator
{
    class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

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
                      services.AddRivenModule<CompanyProjectMigratorModule>(configuration);
                  });
        }



        /// <summary>
        /// 创建 SerilogLogger 配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var cfg = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.File(Path.Join("logs", "log.txt"), rollingInterval: RollingInterval.Hour)
                .WriteTo.Console();


            return cfg.CreateLogger();
        }


        /// <summary>
        /// 获取 appsettings 配置
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
