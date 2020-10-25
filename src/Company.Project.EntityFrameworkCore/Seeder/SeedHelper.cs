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

namespace Company.Project.Seeder
{
    /// <summary>
    /// 种子数据帮助器
    /// </summary>
    public static class SeedHelper
    {
        static bool _skipSeedDb;

        /// <summary>
        /// 是否跳过执行 <see cref="SeedHelper.SeedDb"/> 或 <see cref="SeedHelper.SeedDbAsync"/> 
        /// </summary>
        public static bool SkipSeedDb
        {
            get => SeedHelper._skipSeedDb;
            set
            {
                if (value)
                {
                    SeedHelper._skipSeedDb = value;
                }
            }
        }

        /// <summary>
        /// 创建种子数据
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SeedDb(IServiceProvider serviceProvider)
        {
            if (SkipSeedDb)
            {
                return;
            }

            AsyncHelper.RunSync(async () =>
            {
                await SeedDbAsync(serviceProvider);
            });
        }

        /// <summary>
        /// 创建种子数据 异步
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static async Task SeedDbAsync(IServiceProvider serviceProvider)
        {
            if (SkipSeedDb)
            {
                return;
            }

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {

                    var scopeServiceProvider = scope.ServiceProvider;

                    var unitOfWorkManager = scopeServiceProvider.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {

                        var appContext = unitOfWorkManager.Current.GetDbContext<AppDbContext>();

                        var hostSeeder = scopeServiceProvider.GetService<IHostSeeder>();
                        var defaultTenant = await hostSeeder.Create(appContext);


                        var tenantSeeder = scopeServiceProvider.GetService<ITenantSeeder>();
                        await tenantSeeder.Create(appContext, defaultTenant.Name);

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
