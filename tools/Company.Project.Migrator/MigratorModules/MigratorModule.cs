using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Riven;
using Riven.Modular;
using Company.Project.Authorization;


namespace Company.Project.MigratorModules
{
    [DependsOn(
        typeof(CompanyProjectEntityFrameworkCoreMigratorModule)
        )]
    public class MigratorModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 模块依赖了 asp.net core identiy 相关
            context.Services.IdentityRegister();

            // 添加迁移服务
            context.Services.AddHostedService<MigratorHostedService>();
        }
    }
}
