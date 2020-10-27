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
    /// <summary>
    /// Migrator使用的 Module 依赖
    /// </summary>
    [DependsOn(
        typeof(CompanyProjectDomainModule)
        )]
    public class CompanyProjectEntityFrameworkCoreMigratorModule : CompanyProjectEntityFrameworkCoreModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            base.OnPreConfigureServices(context);
        }

        public override void OnPostConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
