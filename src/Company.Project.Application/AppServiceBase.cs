using System;

using Company.Project.Authorization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Riven;
using Riven.Application;
using Riven.Localization;
using Riven.Uow;

namespace Company.Project
{
    public abstract class AppServiceBase : ApplicationServiceBase
    {
        protected AppServiceBase(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            this.AppSession = this.GetService<IAppSession>();
        }

         protected virtual IAppSession AppSession { get; }
    }
}
