using Microsoft.AspNetCore.Http;

using Riven.MultiTenancy;
using Riven.Extensions;
using Company.Project;


namespace Riven.Uow
{
    /// <summary>
    /// 多租户信息提供者
    /// </summary>
    public class AspNetCoreMultiTenancyProvider : IMultiTenancyProvider
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMultiTenancyOptions _multiTenancyOptions;

        public AspNetCoreMultiTenancyProvider(IHttpContextAccessor httpContextAccessor, IMultiTenancyOptions multiTenancyOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _multiTenancyOptions = multiTenancyOptions;
        }

        public virtual string CurrentTenantNameOrNull()
        {
            // 未启用多租户,则返回系统默认创建的多租户名称
            if (_multiTenancyOptions.IsEnabled)
            {
                // 启用多租户，返回当前请求头中携带的租户名称
                return _httpContextAccessor?.HttpContext?.Request?.Headers?.GetTenantName();

            }
            return AppConsts.MultiTenancy.DefaultTenantName;
        }
    }
}
