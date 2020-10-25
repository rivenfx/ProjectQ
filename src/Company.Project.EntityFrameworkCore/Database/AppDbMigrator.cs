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

namespace Company.Project.Database
{
    public class AppDbMigrator : IDbMigrator, ITransientDependency
    {
        readonly IUnitOfWorkManager _unitOfWorkManager;
        readonly IServiceProvider _serviceProvider;

        public AppDbMigrator(IUnitOfWorkManager unitOfWorkManager, IServiceProvider serviceProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _serviceProvider = serviceProvider;
        }

        public void CreateOrMigrateForHost()
        {
            CreateOrMigrateForHostAsync().GetAwaiter().GetResult();
        }

       

        public void CreateOrMigrateForTenant(string tenantName)
        {
            CreateOrMigrateForHostAsync(tenantName).GetAwaiter().GetResult();
        }

        public async Task CreateOrMigrateForHostAsync()
        {
            await CreateOrMigrateForHostAsync(null);
        }

        public async Task CreateOrMigrateForTenantAsync(string tenantName)
        {
            if (tenantName.IsNullOrWhiteSpace())
            {
                await this.CreateOrMigrate(null, async (appContext) =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var hostSeeder = scope.ServiceProvider.GetRequiredService<IHostSeeder>();
                        var defaultTenant = await hostSeeder.Create(appContext);

                        var tenantSeeder = scope.ServiceProvider.GetRequiredService<ITenantSeeder>();
                        await tenantSeeder.Create(appContext, defaultTenant.Name);
                    }
                });
                return;
            }

            await this.CreateOrMigrate(tenantName, async (appContext) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tenantSeeder = scope.ServiceProvider.GetRequiredService<ITenantSeeder>();
                    await tenantSeeder.Create(appContext, tenantName);
                }
            });
        }

        protected virtual async Task CreateOrMigrate(string tenantName, Func<DbContext, Task> seedAction = null)
        {
            var unitOfWorkOptions = new UnitOfWorkOptions();

            // 工作单元级别
            unitOfWorkOptions.Scope = TransactionScopeOption.Suppress;
            // 当前连接字符串名称
            unitOfWorkOptions.ConnectionStringName = tenantName.IsNullOrWhiteSpace() ? RivenUnitOfWorkConsts.DefaultConnectionStringName : tenantName;


            using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                // 获取当前数据库上下文
                var dbContext = _unitOfWorkManager.Current.GetDbContext();
                dbContext.Database.Migrate();

                // 种子数据
                await seedAction?.Invoke(dbContext);

                // 保存
                _unitOfWorkManager.Current.SaveChanges();

                // 提交工作单元
                await uow.CompleteAsync();
            }
        }
    }
}