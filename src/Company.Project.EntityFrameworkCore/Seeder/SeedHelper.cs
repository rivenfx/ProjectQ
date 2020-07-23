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
        public static void SeedDb(IServiceProvider serviceProvider)
        {
            AsyncHelper.RunSync(async () =>
            {
                await SeedDbAsync(serviceProvider);
            });
        }


        static async Task SeedDbAsync(IServiceProvider serviceProvider)
        {
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
                        await hostSeeder.Create(appContext);


                        var tenantSeeder = scopeServiceProvider.GetService<ITenantSeeder>();
                        await tenantSeeder.Create(appContext);
                     
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
