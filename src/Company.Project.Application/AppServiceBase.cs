using System;

using Company.Project.Authorization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Riven.Application;
using Riven.Localization;
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
            this.AppSession = _serviceProvider.GetRequiredService<IAppSession>();
            this.Configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            this.LanguageManager = _serviceProvider.GetRequiredService<ILanguageManager>();
            this.CurrentLanguage = _serviceProvider.GetRequiredService<ICurrentLanguage>();
        }

        protected virtual IUnitOfWorkManager UnitOfWorkManager { get; }
        protected virtual IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;
        protected virtual IAppSession AppSession { get; }
        protected virtual IConfiguration Configuration { get; }
        protected virtual ILanguageManager LanguageManager { get; }
        protected virtual ICurrentLanguage CurrentLanguage { get; }


        protected virtual TService GetService<TService>()
        {
            return this._serviceProvider.GetRequiredService<TService>();
        }
    }
}
