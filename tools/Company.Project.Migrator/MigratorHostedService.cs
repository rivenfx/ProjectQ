using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Riven;
using Riven.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Riven.Uow;
using System.Transactions;
using Company.Project.MultiTenancy;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Operation.Buffer;

namespace Company.Project
{
    public class MigratorHostedService : IHostedService
    {
        readonly IServiceProvider _serviceProvider;

        readonly ILogger<MigratorHostedService> _logger;

        readonly IHostApplicationLifetime _hostApplicationLifetime;

        readonly IUnitOfWorkManager _unitOfWorkManager;

        readonly IDbMigrator _dbMigrator;

        public MigratorHostedService(IServiceProvider serviceProvider, ILogger<MigratorHostedService> logger, IHostApplicationLifetime hostApplicationLifetime, IUnitOfWorkManager unitOfWorkManager, IDbMigrator dbMigrator)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _unitOfWorkManager = unitOfWorkManager;
            _dbMigrator = dbMigrator;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.UseRivenModule();


            // 宿主数据库升级
            _logger.LogInformation("Host数据库 正在升级");
            await this._dbMigrator.CreateOrMigrateForHostAsync();
            _logger.LogInformation("Host数据库 升级完成");
            if (!MultiTenancyConfig.IsEnabled)
            {
                _logger.LogInformation("未启用多租户,跳过执行租户数据库升级");
                goto end;
            }

            // 租户数据库升级
            var tenants = default(List<Tenant>);
            using (var uowHandle = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantManager = _serviceProvider.GetRequiredService<ITenantManager>();
                tenants = await tenantManager.QueryAsNoTracking.ToListAsync(cancellationToken);
            }
            _logger.LogInformation("开始升级 Tenant 数据库");
            foreach (var tenant in tenants)
            {
                if (string.IsNullOrWhiteSpace(tenant.ConnectionString))
                {
                    continue;
                }

                _logger.LogInformation($"Tenant {tenant.Name} 数据库 正在升级");
                await this._dbMigrator.CreateOrMigrateForTenantAsync(tenant.Name);
                _logger.LogInformation($"Tenant {tenant.Name} 数据库 升级完成");
            }

            end:
            _logger.LogInformation("数据库升级结束!");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
