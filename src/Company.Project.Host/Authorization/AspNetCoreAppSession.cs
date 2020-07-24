using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

using Company.Project.Authorization.AppClaims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Riven.Dependency;
using Riven.Extensions;
using Riven.Localization;
using Riven.Uow;

namespace Company.Project.Authorization
{
    public class AspNetCoreAppSession : IAppSession
    {
        public long? UserId => this.GetUserId();

        public string UserIdString => this.GetUserIdString();

        public string UserName => _httpContextAccessor?.HttpContext?.User.GetUserName(_options.Value);

        public string TenantName => _currentConnectionStringNameProvider.Current;

        public long? ImpersonatedUserId => this.GetImpersonatedUserId();

        public string ImpersonatedUserIdString => this.GetImpersonatedUserIdString();

        public string ImpersonatedTenantName => this.GetImpersonatedTenantNameString();

        public LanguageInfo CurrentLanguage => _currentLanguage.GetCurrentLanguage();

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IOptions<IdentityOptions> _options;
        readonly ICurrentLanguage _currentLanguage;
        readonly ICurrentConnectionStringNameProvider _currentConnectionStringNameProvider;

        public AspNetCoreAppSession(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> options, ICurrentLanguage currentLanguage, ICurrentConnectionStringNameProvider currentConnectionStringNameProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
            _currentLanguage = currentLanguage;
            _currentConnectionStringNameProvider = currentConnectionStringNameProvider;
        }

        #region 用户Id获取函数

        long? GetUserId()
        {
            var userIdString = this.GetUserIdString();
            if (!userIdString.IsNullOrWhiteSpace())
            {
                return long.Parse(userIdString);
            }


            return null;
        }

        string GetUserIdString()
        {
            return _httpContextAccessor?.HttpContext?.User.GetUserId(_options.Value);
        }

        #endregion


        #region 用户Id获取 - 模拟登录

        long? GetImpersonatedUserId()
        {
            var userIdString = this.GetImpersonatedUserIdString();
            if (!userIdString.IsNullOrWhiteSpace())
            {
                return long.Parse(userIdString);
            }


            return null;
        }

        string GetImpersonatedUserIdString()
        {

            var userIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(AppClaimTypes.ImpersonatedUserIdNameIdentifier);

            if (!userIdString.IsNullOrWhiteSpace())
            {
                return userIdString;
            }


            return null;
        }
        #endregion

        #region 租户名称获取函数 - 模拟登录


        string GetImpersonatedTenantNameString()
        {
            var tenantIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(AppClaimTypes.ImpersonatedTenantNameIdentifier);

            if (!tenantIdString.IsNullOrWhiteSpace())
            {
                return tenantIdString;
            }


            return null;
        }

        #endregion



    }
}
