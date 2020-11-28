using System;
using Microsoft.Extensions.DependencyInjection;
using Riven.Application;
using Riven.Uow;

namespace Company.Project
{
    public abstract class AppServiceBase : IApplicationService
    {
        protected readonly IServiceProvider _serviceProvider;

        protected AppServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            this.UnitOfWorkManager = _serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        protected virtual IUnitOfWorkManager UnitOfWorkManager { get; }
        protected virtual IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

    }
}
