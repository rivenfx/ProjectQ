using Company.Project.Seeder;
using Company.Project.Authenticate;
using Company.Project.Authorization;
using Riven.Modular;

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectEntityFrameworkCoreModule)
        )]
    public class CompanyProjectMigratorModule : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            // 跳过执行 SeedHelper.SeedDb 或 SeedHelper.SeedDbAsync
            SeedHelper.SkipSeedDb = true;
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 模块依赖了 asp.net core identiy 相关
            context.Services.IdentityRegister();

            // 添加迁移服务
            context.Services.AddHostedService<MigratorHostedService>();
        }
    }
}
