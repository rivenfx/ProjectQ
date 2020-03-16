using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Company.Project.Database;
using Company.Project.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Riven;
using Riven.Modular;
using Riven.Repositories;
using Riven.Uow;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectDomainModule)
        )]
    public class CompanyProjectEntityFrameworkCoreModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 添加默认的数据库连接字符串
            context.Services.AddDefaultConnectionString(context.Configuration["ConnectionStrings:Default"]);
            // 添加其它连接字符串
            //context.Services.AddConnectionString("TenantA", context.Configuration["ConnectionStrings:Default"]);

            // 添加 efcore 工作单元和仓储实现
            context.Services.AddUnitOfWorkWithEntityFrameworkCore();
            context.Services.AddUnitOfWorkWithEntityFrameworkCoreRepository();

            // 注册默认DbContext
            context.Services.AddUnitOfWorkWithEntityFrameworkCoreDefaultDbContext<AppDbContext>((config) =>
            {
                // 这个在每次需要创建DbContext的时候执行
                if (config.ExistingConnection != null)
                {
                    config.DbContextOptions.Configure(config.ExistingConnection);
                }
                else
                {
                    config.DbContextOptions.Configure(config.ConnectionString);
                }
            });

            //// 添加其它DbContext
            //context.Services.AddUnitOfWorkWithEntityFrameworkCoreDbContext<AppDbContext>("other",(config) =>
            //{
            //    // 这个在每次需要创建DbContext的时候执行
            //    if (config.ExistingConnection != null)
            //    {
            //        config.DbContextOptions.Configure(config.ExistingConnection);
            //    }
            //    else
            //    {
            //        config.DbContextOptions.Configure(config.ConnectionString);
            //    }
            //});
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }
    }
}
