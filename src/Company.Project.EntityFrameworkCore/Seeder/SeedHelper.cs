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
using Riven.Data;
using Riven;

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
                using var scope = serviceProvider.CreateScope();
                var scopeServiceProvider = scope.ServiceProvider;

                // 工作单元管理器
                var unitOfWorkManager = scopeServiceProvider.GetService<IUnitOfWorkManager>();


                // 启动工作单元
                using var uow = unitOfWorkManager.Begin();

                // 当前工作单元
                var currentUow = unitOfWorkManager.Current;

                // 种子数据初始化器
                var dataSeeder = scopeServiceProvider.GetService<IDataSeeder>();

                // 初始化宿主数据
                await dataSeeder.Run(new DataSeedContext());

                // 初始化租户数据
                using (currentUow.ChangeTenant(AppConsts.MultiTenancy.DefaultTenantName))
                {
                    await dataSeeder.Run(new DataSeedContext(AppConsts.MultiTenancy.DefaultTenantName));
                }

                // 提交工作单元
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}