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
    public static class SeedHelper
    {
        /// <summary>
        /// 跳过种子数据
        /// </summary>
        public static bool SkipSeedDb { get; set; }

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


        static async Task SeedDbAsync(IServiceProvider serviceProvider)
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
