using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Riven.Dependency;
using Riven.Extensions;
using Riven.Localization;

namespace Company.Project.Authorization
{
    public class AspNetCoreAppSession : IAppSession, ITransientDependency
    {
        public long? UserId => this.GetUserId();

        public string UserName => _httpContextAccessor?.HttpContext?.User.GetUserName(_options.Value);

        public LanguageInfo CurrentLanguage => _currentLanguage.GetCurrentLanguage();

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IOptions<IdentityOptions> _options;
        readonly ICurrentLanguage _currentLanguage;

        public AspNetCoreAppSession(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> options, ICurrentLanguage currentLanguage)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
            _currentLanguage = currentLanguage;
        }

        long? GetUserId()
        {
            long? userId = null;

            var userIdString = _httpContextAccessor?.HttpContext?.User.GetUserId(_options.Value);
            if (!userIdString.IsNullOrWhiteSpace())
            {
                userId = long.Parse(userIdString);
            }

            return userId;
        }
    }
}
