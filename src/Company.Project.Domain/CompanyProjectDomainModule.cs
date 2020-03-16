using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Riven;
using Riven.Modular;

namespace Company.Project
{
    [DependsOn(

        )]
    public class CompanyProjectDomainModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
