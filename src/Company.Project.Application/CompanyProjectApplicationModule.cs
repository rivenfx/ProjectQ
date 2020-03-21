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
    public class CompanyProjectApplicationModule : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectApplicationModule>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            base.OnConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
