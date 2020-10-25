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
using Riven.Threading;
using Company.Project.Configuration;
using Company.Project.Seeder;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectDomainModule)
        )]
    public class CompanyProjectEntityFrameworkCoreModule : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectEntityFrameworkCoreModule>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {


            #region 添加默认的数据库连接字符串

            context.Services.AddDefaultConnectionString(
                    context.Configuration.GetDefaultDatabaseConnectionString()
                );

            #endregion



            #region 添加其它数据库连接字符串

            //context.Services.AddConnectionString("TenantA", context.Configuration["ConnectionStrings:TenantA"]);

            #endregion



            #region 添加 efcore 工作单元和仓储实现

            context.Services.AddUnitOfWorkWithEntityFrameworkCore();
            context.Services.AddUnitOfWorkWithEntityFrameworkCoreRepository();

            #endregion



            #region 添加默认DbContext

            context.Services.AddUnitOfWorkWithEntityFrameworkCoreDefaultDbContext<AppDbContext>((config) =>
              {
                  // 这个在每次需要创建DbContext的时候执行
                  if (config.ExistingConnection != null)
                  {
                      config.DbContextOptions
                            .Configure(context.Configuration,config.ExistingConnection);
                  }
                  else
                  {
                      config.DbContextOptions
                            .Configure(context.Configuration,config.ConnectionString);
                  }
              });

            #endregion



            #region 添加其它DbContext

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

            #endregion

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            #region 添加数据库连接字符串,在这里添加的优先级高于前面注册的

            //context.ServiceProvider.AddConnectionString("TenantA", "null");

            #endregion


            // 种子数据
            SeedHelper.SeedDb(context.ServiceProvider);
        }
    }
}
