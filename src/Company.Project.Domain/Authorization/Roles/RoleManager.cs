using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Riven;
using Riven.Authorization;
using Riven.Exceptions;
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
        /// ������ɫ
        /// </summary>
        /// <param name="name">��ɫ����(Ψһ����)</param>
        /// <param name="displayName">��ʾ����</param>
        /// <param name="description">����</param>
        /// <param name="claims">��ɫӵ�е�claim����</param>
        /// <returns></returns>
        public async Task<Role> CreateAsync([NotNull]string name, [NotNull]string displayName, string description, List<string> claims)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            var role = new Role()
            {
                Name = name,
                DispayName = displayName,
                Description = description ?? string.Empty
            };
            var result = await this.CreateAsync(role);
            if (!result.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("������ɫʱ��������", detiles.ToString());
            }

            await this.AddIdentityClaimsAsync(role, claims.ToArray());

            return role;
        }

        /// <summary>
        /// ���½�ɫ
        /// </summary>
        /// <param name="id">��ɫid</param>
        /// <param name="displayName">��ʾ����</param>
        /// <param name="description">����</param>
        /// <param name="claims">��ɫӵ�е�claim����</param>
        /// <returns></returns>
        public async Task<Role> UpdateAsync(long? id, [NotNull]string displayName, string description, List<string> claims)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            var role = await this.FindByIdAsync(id.Value.ToString());
            if (role == null)
            {
                throw new UserFriendlyException($"δ�ҵ���ɫ: {displayName}");
            }

            role.DispayName = displayName;
            role.Description = description ?? string.Empty;

            var result = await this.UpdateAsync(role);
            if (!result.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("������ɫʱ��������", detiles.ToString());
            }

            await this.AddIdentityClaimsAsync(role, claims.ToArray());

            return role;
        }


        /// <summary>
        /// ����ɫ��� identity �� claims
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
        /// �޸Ľ�ɫӵ�е� claims
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

            // �Ƴ���ɫ��ǰӵ�е� identity claims
            await this.ClearIdentityClaimsAsync(role);


            // �����cliamsΪ��, �Ƴ���ɫ��ǰӵ�е� identity claims
            if (claims != null && claims.Length > 0)
            {
                // ��ӽ�ɫӵ�е� identity claims
                await this.AddIdentityClaimsAsync(role, claims);
            }
        }

        /// <summary>
        /// �Ƴ���ɫӵ�е����� Identity Claims
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task ClearIdentityClaimsAsync(Role role)
        {
            Check.NotNull(role, nameof(role));


            // ��ɫ��ǰӵ�е� claims
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
            return await this.GetClaimsAsync(role);
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
            return new Claim(cliam, cliam);
        }

        public IEnumerable<Claim> CreateIdentityClaims(string[] claims)
        {
            foreach (var claim in claims)
            {
                yield return new Claim(claim, claim);
            }
        }
    }
}
