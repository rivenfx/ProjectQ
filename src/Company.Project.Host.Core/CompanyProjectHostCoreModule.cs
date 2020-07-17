using Riven.Modular;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Riven.Uow;
using Riven;
using Company.Project.Authorization;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectApplicationModule),
        typeof(CompanyProjectEntityFrameworkCoreModule)
        )]
    public class CompanyProjectHostCoreModule : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectHostCoreModule>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }
    }
}
