using Microsoft.Extensions.DependencyInjection;
using Riven.Threading;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Riven.Uow.Extensions;
using Riven.Extensions;
using Company.Project.Database;
using Riven.MultiTenancy;

namespace Company.Project.Seeder
{
    /// <summary>
    /// 种子数据帮助器
    /// </summary>
    public static class SeedHelper
    {
        /// <summary>
        /// 创建种子数据
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SeedDb(IServiceProvider serviceProvider)
        {
            AsyncHelper.RunSync(async () => { await SeedDbAsync(serviceProvider); });
        }

        /// <summary>
        /// 创建种子数据 异步
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static async Task SeedDbAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopeServiceProvider = scope.ServiceProvider;

                    var unitOfWorkManager = scopeServiceProvider.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                        var currentUow = unitOfWorkManager.Current;


                        var hostContext = currentUow.GetDbContext<AppDbContext>();

                        var hostSeeder = scopeServiceProvider.GetService<IHostSeeder>();
                        var tenant = await hostSeeder.Create(hostContext);

                        using (currentUow.SetConnectionStringName(tenant.Name))
                        {
                            var tenantContext = currentUow.GetDbContext<AppDbContext>();
                            var tenantSeeder = scopeServiceProvider.GetService<ITenantSeeder>();
                            await tenantSeeder.Create(
                                tenantContext,
                                tenant.Name
                            );
                        }


                        await uow.CompleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}