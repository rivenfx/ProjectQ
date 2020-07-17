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

namespace Company.Project.Authorization
{
    public class AspNetCoreAppSession : IAppSession
    {
        public long? UserId => this.GetUserId();

        public string UserIdString => this.GetUserIdString();

        public string UserName => _httpContextAccessor?.HttpContext?.User.GetUserName(_options.Value);

        public LanguageInfo CurrentLanguage => _currentLanguage.GetCurrentLanguage();

        public long? TenantId => this.GetTenantId();

        public string TenantIdString => this.GetTenantIdString();

        public long? ImpersonatedUserId => this.GetImpersonatedUserId();

        public string ImpersonatedUserIdString => this.GetImpersonatedUserIdString();

        public long? ImpersonatedTenantId => this.GetImpersonatedTenantId();

        public string ImpersonatedTenantIdString => this.GetImpersonatedTenantIdString();

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IOptions<IdentityOptions> _options;
        readonly ICurrentLanguage _currentLanguage;

        public AspNetCoreAppSession(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> options, ICurrentLanguage currentLanguage)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
            _currentLanguage = currentLanguage;
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
            var userIdString = _httpContextAccessor?.HttpContext?.User.GetUserId(_options.Value);
            if (!userIdString.IsNullOrWhiteSpace())
            {
                return userIdString;
            }

            userIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!userIdString.IsNullOrWhiteSpace())
            {
                return userIdString;
            }


            return null;
        }

        #endregion


        #region 租户Id获取函数

        long? GetTenantId()
        {
            var tenantIdString = this.GetTenantIdString();
            if (!tenantIdString.IsNullOrWhiteSpace())
            {
                return long.Parse(tenantIdString);
            }


            return null;
        }

        string GetTenantIdString()
        {
            var tenantIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(AppClaimTypes.TenantIdNameIdentifier);

            if (!tenantIdString.IsNullOrWhiteSpace())
            {
                return tenantIdString;
            }


            return null;
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

        #region 租户Id获取函数 - 模拟登录

        long? GetImpersonatedTenantId()
        {
            var tenantIdString = this.GetImpersonatedTenantIdString();
            if (!tenantIdString.IsNullOrWhiteSpace())
            {
                return long.Parse(tenantIdString);
            }


            return null;
        }

        string GetImpersonatedTenantIdString()
        {
            var tenantIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(AppClaimTypes.ImpersonatedTenantIdNameIdentifier);

            if (!tenantIdString.IsNullOrWhiteSpace())
            {
                return tenantIdString;
            }


            return null;
        }

        #endregion



    }
}
