using Microsoft.Extensions.DependencyInjection;
using Riven;
using Riven.Modular;
using System;
using System.Collections.Generic;
using System.Text;
using Company.Project.Authorization.Users;
using Company.Project.Authorization.Users.Dtos;

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

            // зЂВс
            this.GetType().Assembly.RegisterGlobalObjectMapper();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //ConfigurationMapper(context.Services);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
