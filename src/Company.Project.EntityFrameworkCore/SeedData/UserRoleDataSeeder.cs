using Riven.Data;
using Riven.Uow.Providers;
using Riven;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riven.MultiTenancy;
using Company.Project.Authorization.Users;
using Riven.Extensions;
using Company.Project.Authorization.Roles;
using System;
using Riven.Repositories;

namespace Company.Project.SeedData
{
    /// <summary>
    /// 用户角色种子数据创建器
    /// </summary>
    public class UserRoleDataSeeder : IDataSeedExecutor
    {
        protected readonly UserManager _userManager;
        protected readonly RoleManager _roleManager;
        protected readonly IRepository<UserRole, Guid> _userRoleRepo;


        public UserRoleDataSeeder(UserManager userManager, RoleManager roleManager, IRepository<UserRole, Guid> userRoleRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoleRepo = userRoleRepo;
        }

        public async Task Run(DataSeedContext dataSeedContext)
        {
            var userName = AppConsts.Authorization.SystemUserName;
            var userPassword = AppConsts.Authorization.SystemUserPassword;
            var userEmail = "msmadaoe@msn.com";
            var userPhoneNumber = "13000000007";


            #region 从上下文中获取参数

            if (dataSeedContext.Properties.TryGetValue(AppConsts.Authorization.AdminUserName, out var val) && val != null)
            {
                userName = val.ToString();
            }

            if (dataSeedContext.Properties.TryGetValue(AppConsts.Authorization.AdminUserPassword, out val) && val != null)
            {
                userPassword = val.ToString();
            }

            if (dataSeedContext.Properties.TryGetValue(AppConsts.Authorization.AdminUserEmail, out val) && val != null)
            {
                userEmail = val.ToString();
            }

            if (dataSeedContext.Properties.TryGetValue(AppConsts.Authorization.AdminUserPhoneNumber, out val) && val != null)
            {
                userPhoneNumber = val.ToString();
            } 

            #endregion


            // 创建用户
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Nickname = userName,
                    PhoneNumber = userPhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = userEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    IsStatic = true,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    TenantName = dataSeedContext.TenantName,
                };

                user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
                user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);

                user.PasswordHash = _userManager.PasswordHasher
                    .HashPassword(user, userPassword);


                var identityResult = await _userManager.CreateAsync(user);
                identityResult.CheckError();
            }

            // 创建角色
            var role = await _roleManager.FindByNameAsync(AppConsts.Authorization.SystemRoleName);
            if (role == null)
            {
                role = new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = AppConsts.Authorization.SystemRoleName,
                    DisplayName = "系统管理员",
                    NormalizedName = this._roleManager.NormalizeKey(AppConsts.Authorization.SystemRoleName),
                    IsStatic = true,
                    TenantName = dataSeedContext.TenantName
                };

                var identityResult = await _roleManager.CreateAsync(role);
                identityResult.CheckError();
            }

            // 给用户设置角色
            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                var identityResult = await _userManager.AddToRoleAsync(user, role.Name);
                identityResult.CheckError();
            }

        }
    }
}
