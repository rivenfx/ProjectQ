using Company.Project;

using Microsoft.AspNetCore.Http;

using Riven.MultiTenancy;
using Riven.Extensions;


namespace Riven.Uow
{
    public class AspNetCoreCurrentConnectionStringNameProvider : ICurrentConnectionStringNameProvider
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMultiTenancyOptions _multiTenancyOptions;

        public AspNetCoreCurrentConnectionStringNameProvider(IHttpContextAccessor httpContextAccessor, IMultiTenancyOptions multiTenancyOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _multiTenancyOptions = multiTenancyOptions;
        }

        public string Current => this.GetRequestTenantName();

        /// <summary>
        /// 获取请求头中的租户名称
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRequestTenantName()
        {
            // 未启用多租户,则返回系统默认创建的租户名称
            if (!_multiTenancyOptions.IsEnabled)
            {
                return AppConsts.MultiTenancy.DefaultTenantName;
            }

            // 启用多租户，返回当前请求头中携带的租户名称
            return _httpContextAccessor?.HttpContext?.Request?.Headers?.GetTenantName();
        }
    }
}
