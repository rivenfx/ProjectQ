using AspNetCore.Authentication.ApiToken.Abstractions;

using Company.Project.Authorization.Users;

using Microsoft.AspNetCore.Identity;

using Riven.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Company.Project.Authorization.Tokens;

namespace Company.Project.Authorization.Tokens
{
    public class AppTokenProfileService : IApiTokenProfileService
    {
        protected readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        protected readonly UserManager _userManager;

        public AppTokenProfileService(IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory, UserManager userManager)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
        }

        public virtual async Task<List<Claim>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var claimsPrincipal = await this._userClaimsPrincipalFactory.CreateAsync(user);
            return claimsPrincipal.Claims.ToList();
        }
    }
}
