using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Riven;
using Riven.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Roles
{
    public class RoleManager : RoleManager<Role>, IRoleClaimAccessor
    {
        static IList<Claim> _nullRoleClaims = new List<Claim>();


        public IQueryable<Role> Query => this.Roles;

        public RoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager> logger
            ) : base(store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {

        }

        /// <summary>
        /// 给角色添加 identity 的 claims
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task AddIdentityClaimsAsync(Role role, params string[] claims)
        {
            if (role == null || claims == null || claims.Length == 0)
            {
                return;
            }

            foreach (var claim in CreateIdentityClaims(claims))
            {
                await this.AddClaimAsync(role, claim);
            }
        }

        /// <summary>
        /// 修改角色拥有的 claims
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task ChangeIdentityClaimsAsync(Role role, params string[] claims)
        {
            if (role == null)
            {
                return;
            }


            // 输入的cliams为空, 移除角色当前拥有的 identity claims
            if (claims == null || claims.Length == 0)
            {
                await this.ClearIdentityClaimsAsync(role);

                return;
            }

            // 移除角色当前拥有的 identity claims
            await this.ClearIdentityClaimsAsync(role);

            // 添加角色拥有的 identity claims
            await this.AddIdentityClaimsAsync(role, claims);

        }

        /// <summary>
        /// 移除角色拥有的所有 Identity Claims
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task ClearIdentityClaimsAsync(Role role)
        {
            Check.NotNull(role, nameof(role));


            // 角色当前拥有的 claims
            var currentlyHasClaims = await this.GetIdentityClaimsAsync(role);

            if (currentlyHasClaims == null || currentlyHasClaims.Count == 0)
            {
                return;
            }


            foreach (var claim in currentlyHasClaims)
            {
                await this.RemoveClaimAsync(role, claim);
            }
        }

        public async Task<IList<Claim>> GetIdentityClaimsAsync(Role role)
        {
            var claims = await this.GetClaimsAsync(role);
            return claims?.Where(o => o.Issuer == AppConsts.Identity.Issuer)?.ToList();
        }

        public async Task<IList<Claim>> GetClaimsByRoleIdAsync([NotNull] string roleId)
        {
            Check.NotNullOrWhiteSpace(roleId, nameof(roleId));

            var role = await this.FindByIdAsync(roleId);

            return await this.GetIdentityClaimsAsync(role);

        }

        public async Task<IList<Claim>> GetClaimsByRoleNameAsync([NotNull] string roleName)
        {
            Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

            var role = await this.FindByNameAsync(roleName);

            return await this.GetIdentityClaimsAsync(role);
        }

        public async Task<IList<Claim>> GetClaimsByRoleNamesAsync([NotNull] string[] roleNames)
        {
            Check.NotNull(roleNames, nameof(roleNames));
            if (roleNames.Length == 0)
            {
                return _nullRoleClaims;
            }

            var result = new List<Claim>();



            foreach (var roleName in roleNames)
            {
                var roleClaims = await this.GetClaimsByRoleNameAsync(roleName);

                result.AddRange(roleClaims);
            }

            return result;
        }

        public Claim CreateIdentityClaim(string cliam)
        {
            return new Claim(cliam, cliam, "string", AppConsts.Identity.Issuer);
        }

        public IEnumerable<Claim> CreateIdentityClaims(string[] claims)
        {
            foreach (var claim in claims)
            {
                yield return new Claim(claim, claim, "string", AppConsts.Identity.Issuer);
            }
        }
    }
}
