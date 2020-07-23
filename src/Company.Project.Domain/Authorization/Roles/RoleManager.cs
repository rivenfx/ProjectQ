using Company.Project.Authorization.Extenstions;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

using Riven;
using Riven.Authorization;
using Riven.Exceptions;
using Riven.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Roles
{
    public class RoleManager : RoleManager<Role>, IRoleClaimAccessor
    {
        static IList<Claim> _nullRoleClaims = new List<Claim>();


        public IQueryable<Role> Query => this.Roles.AsNoTracking();
        public IQueryable<Role> QueryAsNoTracking => this.Roles.AsNoTracking();

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
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名称(唯一编码)</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        /// <param name="isStatic">是否为内置</param>
        /// <param name="claims">角色拥有的claim集合</param>
        /// <returns></returns>
        public async Task<Role> CreateAsync([NotNull] string name, [NotNull] string displayName, string description, bool isStatic = false,  params string[] claims)
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
                throw new UserFriendlyException("创建角色时发生错误", detiles.ToString());
            }

            await this.AddIdentityClaimsAsync(role, claims.ToArray());

            return role;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        /// <param name="claims">角色拥有的claim集合</param>
        /// <returns></returns>
        public async Task<Role> UpdateAsync(long? id, [NotNull] string displayName, string description, params string[] claims)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            var role = await this.FindByIdAsync(id.Value.ToString());
            if (role == null)
            {
                throw new UserFriendlyException($"未找到角色: {displayName}");
            }

            role.DispayName = displayName;
            role.Description = description ?? string.Empty;

            var identityResult = await this.UpdateAsync(role);
            if (!identityResult.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in identityResult.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("创建角色时发生错误", detiles.ToString());
            }

            await this.AddIdentityClaimsAsync(role, claims.ToArray());

            return role;
        }


        /// <summary>
        /// 根据条件删除用户
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Role>> DeleteAsync([NotNull] Expression<Func<Role, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));

            var roles = this.Roles.AsNoTracking().Where(o => !o.IsStatic).Where(predicate);
            if ((await roles.CountAsync()) == 0)
            {
                return roles;
            }

            foreach (var role in roles)
            {
                var identityResult = await this.DeleteAsync(role);
                if (!identityResult.Succeeded)
                {
                    var detiles = new StringBuilder();
                    foreach (var error in identityResult.Errors)
                    {
                        detiles.AppendLine($"{error.Code}: {error.Description}");
                    }
                    throw new UserFriendlyException("删除角色失败!", detiles.ToString());
                }
            }

            return roles;
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

            var claimsDistinct = claims.Distinct().Where(o => !o.IsNullOrWhiteSpace());
            if (claimsDistinct.Count() == 0)
            {
                return;
            }

            foreach (var claim in claimsDistinct.ToClaims())
            {
                var identityResult = await this.AddClaimAsync(role, claim);
                if (!identityResult.Succeeded)
                {
                    var detiles = new StringBuilder();
                    foreach (var error in identityResult.Errors)
                    {
                        detiles.AppendLine($"{error.Code}: {error.Description}");
                    }
                    throw new UserFriendlyException("角色添加权限失败!", detiles.ToString());
                }
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

            // 移除角色当前拥有的 identity claims
            await this.ClearIdentityClaimsAsync(role);


            // 输入的cliams为空, 移除角色当前拥有的 identity claims
            if (claims != null && claims.Length > 0)
            {
                // 添加角色拥有的 identity claims
                await this.AddIdentityClaimsAsync(role, claims);
            }
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

        public override string NormalizeKey(string key)
        {
            return key?.ToLower();
        }
    }
}
