using Microsoft.AspNetCore.Http;

using Riven.MultiTenancy;


namespace Riven.Uow
{

    public class AspNetCoreCurrentConnectionStringNameProvider : AspNetCoreMultiTenancyProvider, ICurrentConnectionStringNameProvider
    {
        public AspNetCoreCurrentConnectionStringNameProvider(IHttpContextAccessor httpContextAccessor, IMultiTenancyOptions multiTenancyOptions) : base(httpContextAccessor, multiTenancyOptions)
        {
        }

        public string Current => this.CurrentTenantNameOrNull();
    }
}
