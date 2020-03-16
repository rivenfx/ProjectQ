using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> options
            ) : base(userManager, roleManager, options)
        {
        }

        public override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return base.CreateAsync(user);
        }

        protected override Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            return base.GenerateClaimsAsync(user);
        }
    }
}
