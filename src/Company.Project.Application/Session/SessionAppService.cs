using Company.Project.Authorization;
using Company.Project.Authorization.AppClaims;
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Session
{
    public class SessionAppService : IApplicationService
    {
        readonly IAppSession _appSession;
        readonly UserManager _userManager;
        readonly RoleManager _roleManager;
        readonly IClaimsManager _claimsManager;
        readonly IConfiguration _configuration;
        readonly ILanguageManager _languageManager;


        public SessionAppService(IAppSession appSession, UserManager userManager, RoleManager roleManager, IClaimsManager claimsManager, IConfiguration configuration, ILanguageManager languageManager)
        {
            _appSession = appSession;
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsManager = claimsManager;
            _configuration = configuration;
            _languageManager = languageManager;
        }

        public async Task<SessionDto> GetCurrentSession()
        {
            var appInfo = this._configuration.GetAppInfo();





            return new SessionDto()
            {
                Name = appInfo.Name,
                Version = appInfo.Version,
                Auth = await this.GetClaims(),
                Localization = this.GetLocalization(),
                Menu = this.GetMenu()
            };
        }

        public LocalizationDto GetLocalization()
        {
            var localzation = new LocalizationDto();

            localzation.DefaultCulture = this._languageManager.GetDefaultLanguage().Culture;
            localzation.CurrentCulture = _appSession.CurrentLanguage.Culture;

            localzation.Languages = this._languageManager.GetEnabledLanguages()
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

        public async Task<ClaimsDto> GetClaims()
        {
            var claimsDto = new ClaimsDto();
            claimsDto.AllClaims = this._claimsManager.GetAll().ToList();

            // 已登录
            if (_appSession.UserId.HasValue)
            {
                var userIdString = _appSession.UserId.Value.ToString();

                var userClaims = (await _userManager.GetClaimsByUserIdAsync(userIdString))
                    .Select(o => o.Value);

                var userRoleNames = (await _userManager.GetRolesByUserIdAsync(userIdString));
                var roleClaims = (await _roleManager.GetClaimsByRoleNamesAsync(userRoleNames.ToArray()))
                    .Select(o => o.Value);

                claimsDto.GrantedClaims = userClaims.Union(roleClaims).Distinct().ToList();

            }
            else // 未登录
            {
                claimsDto.GrantedClaims = new List<string>();
            }

            return claimsDto;
        }


        protected virtual string GetMenu()
        {
            return File.ReadAllText("./Menus/menus.json", Encoding.UTF8);
        }
    }
}
