using Microsoft.Extensions.DependencyInjection;
using Riven.Modular;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{
    /// <summary>
    /// 缓存模块
    /// </summary>
    public class EasyCachingModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEasyCaching(options =>
            {
                options.UseInMemory();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
