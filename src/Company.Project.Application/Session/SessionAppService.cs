using Company.Project.Authorization;
using Company.Project.Authorization.Permissions;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Configuration;
using Company.Project.Localization.Dtos;
using Company.Project.MultiTenancy;
using Company.Project.Session.Dtos;

using Microsoft.Extensions.Configuration;

using Nito.AsyncEx;

using Riven;
using Riven.Application;
using Riven.Authorization;
using Riven.Identity.Permissions;
using Riven.Identity.Users;
using Riven.Localization;
using Riven.MultiTenancy;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Session
{
    public class SessionAppService : AppServiceBase
    {
        readonly ITenantManager _tenantManager;
        readonly PermissionManager _permissionManager;
        readonly IMultiTenancyOptions _multiTenancyOptions;
        readonly UserManager _userManager;

        public SessionAppService(
            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            _tenantManager = GetService<ITenantManager>();
            _permissionManager = GetService<PermissionManager>();
            _multiTenancyOptions = GetService<IMultiTenancyOptions>();
            _userManager = GetService<UserManager>();
        }

        public async Task<SessionDto> GetCurrentSession()
        {
            var appInfo = this.Configuration.GetAppInfo();





            return new SessionDto()
            {
                Name = appInfo.Name,
                Version = appInfo.Version,
                MultiTenancy = await this.GetMultiTenancy(),
                Auth = await this.GetAuth(),
                Localization = this.GetLocalization(),
                Menu = this.GetMenu()
            };
        }

        public LocalizationDto GetLocalization()
        {
            var localzation = new LocalizationDto();

            localzation.DefaultCulture = this.LanguageManager.GetDefaultLanguage().Culture;
            localzation.CurrentCulture = AppSession.CurrentLanguage.Culture;

            localzation.Languages = this.LanguageManager.GetEnabledLanguages()
                .Select(o =>
                {
                    var dto = o.MapTo<LanguageInfoDto>();
                    if (localzation.CurrentCulture != dto.Culture)
                    {
                        dto.Texts = null;
                    }

                    return dto;

                }).ToList();

            return localzation;
        }

        public async Task<AuthDto> GetAuth()
        {
            var authDto = new AuthDto();


            #region 当前登录用户信息

            // 用户信息
            if (AppSession.UserId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(AppSession.UserId.ToString());
                authDto.UserId = user.Id.ToString();
                authDto.UserName = user.UserName;
                authDto.UserNickName = user.Nickname;
            }

            #endregion


            #region 系统所有权限

            // 不同情况返回不同的 Permission
            // 租户不为空时
            var all = this._permissionManager.GetSystemItem();
            if (!string.IsNullOrWhiteSpace(AppSession.TenantName) && _multiTenancyOptions.IsEnabled)
            {
                authDto.AllPermissions = all
                    .Where(o => o.Scope != PermissionAuthorizeScope.Host)
                    .Select(o => o.Name)
                    .ToList();
            }
            else
            {
                authDto.AllPermissions = all.Select(o => o.Name).ToList();

            }

            #endregion


            #region 当前登录用户拥有的权限

            // 已登录
            if (AppSession.UserId.HasValue)
            {
                var userIdString = AppSession.UserId.Value.ToString();

                var permissionFinder = this.GetService<IIdentityPermissionFinder>();
                var userRoleFinder = this.GetService<IIdentityUserRoleFinder>();



                var userPermissions = await permissionFinder.FindPermissions(IdentityPermissionType.User, userIdString);

                var userRoleNames = await userRoleFinder.GetRolesByUserIdAsync(userIdString);
                var rolePermissions = await permissionFinder.FindPermissions(IdentityPermissionType.Role, userRoleNames.ToArray());

                authDto.GrantedPermissions = userPermissions.Union(rolePermissions).Distinct().ToList();

            }
            else // 未登录
            {
                authDto.GrantedPermissions = new List<string>();
            }

            #endregion


            return authDto;
        }

        public async Task<MultiTenancyDto> GetMultiTenancy()
        {
            var displayName = await this._tenantManager
                .GetDisplayNameByName(AppSession.TenantName);

            return new MultiTenancyDto()
            {
                IsEnabled = this._multiTenancyOptions.IsEnabled,
                Name = AppSession.TenantName,
                DisplayName = displayName
            };
        }

        protected virtual string GetMenu()
        {
            return File.ReadAllText("./Menus/menus.json", Encoding.UTF8);
        }
    }
}
