using Microsoft.Extensions.Hosting;

using Riven;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Company.Project
{
    public class MigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        readonly IServiceProvider _serviceProvider;

        public MigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.UseRivenModule();
            _hostApplicationLifetime.StopApplication();



            //using (var application = AbpApplicationFactory.Create<YourProjectNameDbMigratorModule>(options =>
            //{
            //    options.UseAutofac();
            //    options.Services.AddLogging(c => c.AddSerilog());
            //}))
            //{
            //    application.Initialize();

            //    await application
            //        .ServiceProvider
            //        .GetRequiredService<YourProjectNameDbMigrationService>()
            //        .MigrateAsync();

            //    application.Shutdown();

            //    _hostApplicationLifetime.StopApplication();
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
