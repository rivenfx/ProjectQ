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

namespace Riven.Uow
{
    public class AspNetCoreCurrentConnectionStringNameProvider : ICurrentConnectionStringNameProvider
    {
        public string Current => _httpContextAccessor?.HttpContext?.Request?.Headers?.GetTenantName();


        readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreCurrentConnectionStringNameProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
