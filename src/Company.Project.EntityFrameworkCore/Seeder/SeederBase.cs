using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Seeder
{
    public abstract class SeederBase
    {
        protected readonly ILookupNormalizer _lookupNormalizer;

        protected readonly IPasswordHasher<User> _passwordHasher;

        public SeederBase(ILookupNormalizer lookupNormalizer, IPasswordHasher<User> passwordHasher)
        {
            _lookupNormalizer = lookupNormalizer;
            _passwordHasher = passwordHasher;
        }


        /// <summary>
        /// 创建系统角色
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="tenantName">租户名称</param>
        /// <returns></returns>
        protected virtual async Task<Role> CreateRoles(DbContext dbContext, string tenantName)
        {
            var roleStore = dbContext.Set<Role>();
            var roleClaimStore = dbContext.Set<RoleClaim>();


            var systemRole = await roleStore.IgnoreQueryFilters()
                .FirstOrDefaultAsync(o => o.Name == AppConsts.Authorization.SystemRoleName);

            if (systemRole == null)
            {
                systemRole = new Role()
                {
                    Name = AppConsts.Authorization.SystemRoleName,
                    DispayName = AppConsts.Authorization.SystemRoleName,
                    Description = AppConsts.Authorization.SystemRoleName,
                    NormalizedName = this._lookupNormalizer.NormalizeName(AppConsts.Authorization.SystemRoleName),
                    IsStatic = true,
                    TenantName = tenantName
                };
                await roleStore.AddAsync(systemRole);
                await dbContext.SaveChangesAsync();
            }

            // 查询现有权限
            var roleClaims = await roleClaimStore.AsQueryable()
                .Where(o => o.Id == systemRole.Id)
                .ToListAsync();

            // 移除权限
            roleClaimStore.RemoveRange(roleClaims);


            // 添加权限
            roleClaims.Clear();
            foreach (var item in AppClaimsConsts.GetClaims())
            {
                roleClaims.Add(new RoleClaim()
                {
                    ClaimType = item,
                    ClaimValue = item
                });
            }
            await roleClaimStore.AddRangeAsync(roleClaims);

            return systemRole;
        }

        /// <summary>
        /// 创建系统用户
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="systemRole">系统默认角色</param>
        /// <returns></returns>
        protected virtual async Task<User> CreateTenantUsers(DbContext dbContext, Role systemRole)
        {
            var userStore = dbContext.Set<User>();
            var userRoleStore = dbContext.Set<UserRole>();

            var systemUser = await userStore.IgnoreQueryFilters()
                .FirstOrDefaultAsync(o => o.UserName == AppConsts.Authorization.SystemUserName);
            if (systemUser == null)
            {
                systemUser = new User()
                {
                    UserName = AppConsts.Authorization.SystemUserName,
                    Nickname = AppConsts.Authorization.SystemUserName,
                    PhoneNumber = "13000000007",
                    PhoneNumberConfirmed = true,
                    Email = "msmadaoe@msn.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    IsStatic = true,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                };

                systemUser.NormalizedUserName = this._lookupNormalizer.NormalizeName(systemUser.UserName);
                systemUser.NormalizedEmail = this._lookupNormalizer.NormalizeName(systemUser.Email);

                systemUser.PasswordHash = this._passwordHasher
                    .HashPassword(systemUser, AppConsts.Authorization.SystemUserPassword);

                await userStore.AddAsync(systemUser);
                await dbContext.SaveChangesAsync();
            }


            // 用户添加角色
            var roles = await userRoleStore.AsQueryable()
                            .Where(o => o.UserId == systemUser.Id)
                            .ToListAsync();
            if (!roles.Any(o => o.RoleId == systemRole.Id))
            {
                await userRoleStore.AddAsync(new UserRole()
                {
                    RoleId = systemRole.Id,
                    UserId = systemUser.Id,
                    TenantName = systemRole.TenantName
                });
            }

            return systemUser;
        }
    }
}
