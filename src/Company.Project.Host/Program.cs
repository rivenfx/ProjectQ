using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using WebHost = Microsoft.Extensions.Hosting.Host;

using Serilog;
using Serilog.Events;
using System.IO;

namespace Company.Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // ������־���������̨
                .WriteTo.Console()
                // ������־������ļ����ļ��������ǰ��Ŀ�� logs Ŀ¼��
                // �ռǵ���������ΪÿСʱ
                .WriteTo.File(Path.Join("logs", "log.txt"), rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            try
            {
                Log.Information("Starting Company.Project.Host.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            })
                            .UseSerilog();
        }

    }
}
