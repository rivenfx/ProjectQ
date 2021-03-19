using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Riven.MultiTenancy;
using Riven.Extensions;
using Riven.Localization;
using Riven.Identity;

namespace Company.Project.Authorization
{
    public class AspNetCoreAppSession : IAppSession
    {
        public Guid? UserId => GetUserId();

        public string UserName => _currentUser.UserName;

        public string TenantName => _multiTenancyProvider.CurrentTenantNameOrNull();

        public Guid? ImpersonatedUserId => this.GetImpersonatedUserId();

        public string ImpersonatedTenantName => this.GetImpersonatedTenantNameString();

        public LanguageInfo CurrentLanguage => _currentLanguage.GetCurrentLanguage();

        readonly ICurrentUser _currentUser;
        readonly IOptions<IdentityOptions> _options;
        readonly ICurrentLanguage _currentLanguage;
        readonly IMultiTenancyProvider _multiTenancyProvider;

        public AspNetCoreAppSession(ICurrentUser currentUser, IOptions<IdentityOptions> options, ICurrentLanguage currentLanguage, IMultiTenancyProvider multiTenancyProvider)
        {
            _currentUser = currentUser;
            _options = options;
            _currentLanguage = currentLanguage;
            _multiTenancyProvider = multiTenancyProvider;
        }


        #region 用户Id获取函数

        Guid? GetUserId()
        {
            var userIdString = _currentUser.UserId;
            if (userIdString.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(userIdString);
        }

        #endregion


        #region 用户Id获取 - 模拟登录


        Guid? GetImpersonatedUserId()
        {
            var userIdString = _currentUser.CurrentPrincipalAccessor.Principal.FindFirstValue(IdentityClaimTypes.ImpersonatedUserIdNameIdentifier);
            if (userIdString.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(userIdString);
        }

        #endregion


        #region 租户名称获取函数 - 模拟登录


        string GetImpersonatedTenantNameString()
        {
            var tenantNameString = _currentUser.CurrentPrincipalAccessor.Principal.FindFirstValue(IdentityClaimTypes.ImpersonatedTenantNameIdentifier);

            if (!tenantNameString.IsNullOrWhiteSpace())
            {
                return tenantNameString;
            }


            return null;
        }

        #endregion


    }
}

