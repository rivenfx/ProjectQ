using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using Riven;
using Riven.AspNetCore.Accessors;
using Riven.Modular;

using Company.Project.Authorization;
using Riven.Extensions;
using System.IO;
using Company.Project.Configuration;
using Riven.Uow;
using Newtonsoft.Json.Converters;
using Company.Project.Debugger;

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
          
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            


        }

        public override void OnPostConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            

        }
    }
}
