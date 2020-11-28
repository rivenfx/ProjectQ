using Company.Project.Authorization;
using Company.Project.Authorization.Permissions;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Configuration;
using Company.Project.Localization.Dtos;
using Company.Project.Session.Dtos;

using Microsoft.Extensions.Configuration;

using Nito.AsyncEx;

using Riven;
using Riven.Application;
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
        readonly UserManager _userManager;
        readonly RoleManager _roleManager;
        readonly IPermissionManager _permissionManager;


        public SessionAppService(
            IServiceProvider serviceProvider,
            UserManager userManager,
            RoleManager roleManager,
            IPermissionManager permissionManager
            ) : base(serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        public async Task<SessionDto> GetCurrentSession()
        {
            var appInfo = this.Configuration.GetAppInfo();





            return new SessionDto()
            {
                Name = appInfo.Name,
                Version = appInfo.Version,
                UserId = AppSession.UserId?.ToString(),
                MultiTenancy = this.GetMultiTenancy(),
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

            // 不同情况返回不同的 Permission
            // 租户不为空时
            var all = this._permissionManager.GetAll();
            if (!string.IsNullOrWhiteSpace(AppSession.TenantName) && MultiTenancyConfig.IsEnabled)
            {
                authDto.AllPermissions = all
                    .Where(o => o.Scope != Riven.Identity.Authorization.PermissionAuthorizeScope.Host)
                    .Select(o => o.Name)
                    .ToList();
            }
            else
            {
                authDto.AllPermissions = all.Select(o => o.Name).ToList();

            }

            // 已登录
            if (AppSession.UserId.HasValue)
            {
                var userIdString = AppSession.UserId.Value.ToString();

                var userPermissions = await _userManager.GetPermissionsByUserIdAsync(userIdString);

                var userRoleNames = await _userManager.GetRolesByUserIdAsync(userIdString);
                var rolePermissions = await _roleManager.GetPermissionsByRoleNamesAsync(userRoleNames.ToArray());

                authDto.GrantedPermissions = userPermissions.Union(rolePermissions).Distinct().ToList();

            }
            else // 未登录
            {
                authDto.GrantedPermissions = new List<string>();
            }

            return authDto;
        }

        public MultiTenancyDto GetMultiTenancy()
        {
            return new MultiTenancyDto()
            {
                IsEnabled = MultiTenancyConfig.IsEnabled,
                TenantName = AppSession.TenantName
            };
        }

        protected virtual string GetMenu()
        {
            return File.ReadAllText("./Menus/menus.json", Encoding.UTF8);
        }
    }
}
