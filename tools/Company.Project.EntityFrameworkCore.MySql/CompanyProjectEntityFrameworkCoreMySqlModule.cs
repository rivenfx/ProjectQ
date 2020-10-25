using Company.Project.Configuration;
using Company.Project.Database;
using Riven;
using Riven.Modular;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{

    [DependsOn(
      typeof(CompanyProjectDomainModule)
      )]
    public class CompanyProjectEntityFrameworkCoreMySqlModule : CompanyProjectEntityFrameworkCoreMigratorModule
    {
        public override void OnPostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services
                  .AddUnitOfWorkWithEntityFrameworkCoreDbContext<AppDbContextForMySql>(
                  nameof(DatabaseType.MySql),
                  (config) =>
                  {
                      // 这个在每次需要创建DbContext的时候执行
                      if (config.ExistingConnection != null)
                      {
                          config.DbContextOptions
                                 .Configure(context.Configuration, config.ExistingConnection);
                      }
                      else
                      {
                          config.DbContextOptions
                                 .Configure(context.Configuration, config.ConnectionString);
                      }
                  });
        }
    }
}
