using Microsoft.AspNetCore.Http;

using Riven.MultiTenancy;
using Riven.Extensions;
using Company.Project;


namespace Riven.Uow
{
    public class AspNetCoreMultiTenancyCurrentConnectionStringNameProvider : ICurrentConnectionStringNameProvider, IMultiTenancyProvider
    {
        public string Current => this.CurrentTenantNameOrNull();


        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IMultiTenancyOptions _multiTenancyOptions;

        public AspNetCoreMultiTenancyCurrentConnectionStringNameProvider(IHttpContextAccessor httpContextAccessor, IMultiTenancyOptions multiTenancyOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _multiTenancyOptions = multiTenancyOptions;
        }

        public virtual string CurrentTenantNameOrNull()
        {
            // 未启用多租户,则返回系统默认创建的多租户名称
            if (this.GetMultiTenancyEnabled())
            {
                // 启用多租户，返回当前请求头中携带的租户名称
                return _httpContextAccessor?.HttpContext?.Request?.Headers?.GetTenantName();

            }
            return AppConsts.MultiTenancy.DefaultTenantName;
        }

        public virtual bool GetMultiTenancyEnabled()
        {
            return _multiTenancyOptions.IsEnabled;
        }
    }
}
