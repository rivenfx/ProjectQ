using System;
using System.Linq;
using Riven.MultiTenancy;
using Riven.Dependency;
using Riven.Uow;
using Riven;
using System.Transactions;
using Riven.Uow.Providers;
using Riven.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Company.Project.Seeder;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Company.Project.Database
{
    public class AppDbMigrator : IDbMigrator, ITransientDependency
    {
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<AppDbMigrator> _logger;
        readonly IUnitOfWorkManager _unitOfWorkManager;

        public AppDbMigrator(IServiceProvider serviceProvider, ILogger<AppDbMigrator> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void CreateOrMigrateForHost()
        {
            CreateOrMigrateForHostAsync().GetAwaiter().GetResult();
        }

        public void CreateOrMigrateForTenant(string tenantName)
        {
            CreateOrMigrateForTenantAsync(tenantName).GetAwaiter().GetResult();
        }

        public async Task CreateOrMigrateForHostAsync()
        {
            await CreateOrMigrateForTenantAsync(null);
        }

        public async Task CreateOrMigrateForTenantAsync(string tenantName)
        {

        }
        //{
        //    if (tenantName.IsNullOrWhiteSpace())
        //    {
        //        await this.CreateOrMigrate(null, async (appContext) =>
        //        {
        //            using (var scope = _serviceProvider.CreateScope())
        //            {
        //                var hostSeeder = scope.ServiceProvider.GetRequiredService<IHostSeeder>();
        //                var defaultTenant = await hostSeeder.Create(appContext);

        //                var tenantSeeder = scope.ServiceProvider.GetRequiredService<ITenantSeeder>();
        //                await tenantSeeder.Create(appContext, defaultTenant.Name);
        //            }
        //        });
        //        return;
        //    }

        //    await this.CreateOrMigrate(tenantName, async (appContext) =>
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var tenantSeeder = scope.ServiceProvider.GetRequiredService<ITenantSeeder>();
        //            await tenantSeeder.Create(appContext, tenantName);
        //        }
        //    });
        //}

        //protected virtual async Task CreateOrMigrate(string tenantName, Func<DbContext, Task> seedAction = null)
        //{
        //    var unitOfWorkOptions = new UnitOfWorkOptions();

        //    // 工作单元级别
        //    unitOfWorkOptions.Scope = TransactionScopeOption.Suppress;
        //    unitOfWorkOptions.IsTransactional = false;
        //    // 当前连接字符串名称
        //    unitOfWorkOptions.ConnectionStringName = tenantName.IsNullOrWhiteSpace() ? RivenUnitOfWorkConsts.DefaultConnectionStringName : tenantName;


        //    using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //    {
        //        // 获取当前数据库上下文
        //        var dbContext = _unitOfWorkManager.Current.GetDbContext();


        //        var migrations = dbContext.Database.GetMigrations().ToList();
        //        var appliedMigrations = dbContext.Database.GetAppliedMigrations().ToList();
        //        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();


        //        // 迁移数据库结构
        //        dbContext.Database.Migrate();


        //        // 种子数据
        //        await seedAction?.Invoke(dbContext);

        //        // 保存
        //        _unitOfWorkManager.Current.SaveChanges();

        //        // 提交工作单元
        //        await uow.CompleteAsync();
        //    }
        //}
    }
}