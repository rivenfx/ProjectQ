using Company.Project.Authorization.Extenstions;
using Company.Project.Authorization.Permissions;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

using Riven;
using Riven.Authorization;
using Riven.Exceptions;
using Riven.Extensions;
using Riven.Identity.Permissions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Roles
{
    public class RoleManager : RoleManager<Role>
    {
        static IEnumerable<string> _emptyRolePermissions = new List<string>();


        public virtual IQueryable<Role> Query => this.Roles.AsNoTracking();
        public virtual IQueryable<Role> QueryAsNoTracking => this.Roles.AsNoTracking();

        protected readonly IIdentityPermissionStore<Permission> _permissionStore;

        public RoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager> logger,
            IIdentityPermissionStore<Permission> permissionStore
            ) : base(store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
            _permissionStore = permissionStore;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名称(唯一编码)</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        /// <param name="isStatic">是否为内置</param>
        /// <param name="permissions">角色拥有的 Permission 集合</param>
        /// <returns></returns>
        public async Task<Role> CreateAsync([NotNull] string name, [NotNull] string displayName, string description, bool isStatic = false, params string[] permissions)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            var role = new Role()
            {
                Name = name,
                DisplayName = displayName,
                Description = description ?? string.Empty
            };
            (await this.CreateAsync(role))
                .CheckError(true, "创建角色时发生错误");

            await this.AddPermissionsAsync(role.Name, permissions.ToArray());

            return role;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        /// <param name="permissions">角色拥有的 Permission 集合</param>
        /// <returns></returns>
        public async Task<Role> UpdateAsync(Guid? id, [NotNull] string displayName, string description, params string[] permissions)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            var role = await this.FindByIdAsync(id.Value.ToString());
            if (role == null)
            {
                throw new UserFriendlyException($"未找到角色: {displayName}");
            }

            role.DisplayName = displayName;
            role.Description = description ?? string.Empty;

            (await this.UpdateAsync(role))
               .CheckError(true, "修改角色时发生错误");

            await this.ChangePermissionsAsync(role.Name, permissions.ToArray());

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

            var roles = await this.Roles.AsNoTracking().Where(o => !o.IsStatic).Where(predicate).ToListAsync();
            if (roles.Count == 0)
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
        /// 获取角色拥有的权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetPermissionsAsync(string roleName)
        {
            Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

            return await this._permissionStore.FindPermissions(
                 IdentityPermissionType.Role,
                 roleName
                 );
        }

        /// <summary>
        /// 给角色添加 identity 的 permission
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task AddPermissionsAsync(string roleName, params string[] permissions)
        {
            Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

            if (permissions == null || permissions.Length == 0)
            {
                return;
            }

            var permissionsDistinct = permissions.Distinct()
                .Where(o => !o.IsNullOrWhiteSpace())
                .Select(o => new Permission()
                {
                    Name = o,
                    Type = IdentityPermissionType.Role,
                    Provider = roleName
                });
            if (permissionsDistinct.Count() == 0)
            {
                return;
            }
            await _permissionStore.CreateAsync(permissionsDistinct);
        }

        /// <summary>
        /// 移除指定的权限
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task RemovePermissionsAsync(string roleName, params string[] permissions)
        {
            Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

            await this._permissionStore.Remove(IdentityPermissionType.Role, roleName, permissions);
        }

        /// <summary>
        /// 修改角色拥有的 Permission
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task ChangePermissionsAsync(string roleName, params string[] permissions)
        {
            Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

            // 当前拥有的权限
            var currrentPermissions = await GetPermissionsAsync(roleName);

            // 当前权限数量为0
            if (currrentPermissions.Count() == 0)
            {
                // 输入权限数量不等于空
                if (!permissions.IsNullOrEmpty())
                {
                    await this.AddPermissionsAsync(roleName, permissions);
                }
                return;
            }



            // 当前用户角色数量大于0

            // 输入角色数据为空,删除用户当前拥有的角色
            if (permissions.IsNullOrEmpty())
            {
                await this.ClearPermissionsAsync(roleName);
                return;
            }


            // 新增的权限
            var addPermissions = permissions.Except(currrentPermissions);
            if (addPermissions.Count() > 0)
            {
                await this.AddPermissionsAsync(roleName, addPermissions.ToArray());
            }

            // 删除的权限
            var removePermissions = currrentPermissions.Except(currrentPermissions);
            if (removePermissions.Count() > 0)
            {
                await this.RemovePermissionsAsync(roleName, removePermissions.ToArray());
            }
        }

        /// <summary>
        /// 移除角色拥有的所有 Permissions
        /// </summary>
        /// <param name="roleName">
        /// 角色名称
        /// </param>
        /// <returns></returns>
        public async Task ClearPermissionsAsync([NotNull] string roleName)
        {
            Check.NotNull(roleName, nameof(roleName));
            await this._permissionStore.Remove(IdentityPermissionType.Role, roleName);
        }
    }
}
