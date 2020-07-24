using Microsoft.AspNetCore.Http;

using Riven.Uow;

using System;
using System.Collections.Generic;
using System.Text;

using Riven.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using JetBrains.Annotations;
using Riven;
using System.Linq;
using Company.Project;
using Riven.MultiTenancy;

namespace Riven.Uow
{
    public class AspNetCoreCurrentConnectionStringNameProvider : ICurrentConnectionStringNameProvider
    {
        public string Current => this.GetCurrentTenantName();


        readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreCurrentConnectionStringNameProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        string GetCurrentTenantName()
        {
            var tenantName = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetTenantName();

            // 启用多租户,并且多租户名称不为空,返回租户名称
            if (MultiTenancyConfig.IsEnabled && !tenantName.IsNullOrWhiteSpace())
            {
                return tenantName;
            }

            // 返回空
            return null;
        }
    }
}
