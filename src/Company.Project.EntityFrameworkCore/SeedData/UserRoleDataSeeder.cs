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

namespace Company.Project.SeedData
{
    /// <summary>
    /// 用户角色种子数据创建器
    /// </summary>
    public class UserRoleDataSeeder : IDataSeedExecutor
    {
        protected readonly IGuidGenerator _guidGenerator;
        protected readonly UserManager _userManager;
        protected readonly RoleManager _roleManager;

        public UserRoleDataSeeder(IGuidGenerator guidGenerator, UserManager userManager, RoleManager roleManager)
        {
            _guidGenerator = guidGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Run(DataSeedContext dataSeedContext)
        {
            // 创建用户
            var user = await _userManager.FindByNameAsync(AppConsts.Authorization.SystemUserName);
            if (user == null)
            {
                user = new User()
                {
                    Id = this._guidGenerator.Create(),
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
                    TenantName = dataSeedContext.TenantName,
                };

                user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
                user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);

                user.PasswordHash = _userManager.PasswordHasher
                    .HashPassword(user, AppConsts.Authorization.SystemUserPassword);


                var identityResult = await _userManager.CreateAsync(user);
                identityResult.CheckError();
            }

            // 创建角色
            var role = await _roleManager.FindByNameAsync(AppConsts.Authorization.SystemRoleName);
            if (role == null)
            {
                role = new Role()
                {
                    Id = this._guidGenerator.Create(),
                    Name = AppConsts.Authorization.SystemRoleName,
                    DisplayName = AppConsts.Authorization.SystemRoleName,
                    Description = AppConsts.Authorization.SystemRoleName,
                    NormalizedName = this._roleManager.NormalizeKey(AppConsts.Authorization.SystemRoleName),
                    IsStatic = true,
                    TenantName = dataSeedContext.TenantName
                };

                var identityResult = await _roleManager.CreateAsync(role);
                identityResult.CheckError();
            }

            // 给用户设置角色
            {
                var identityResult = await _userManager.AddToRoleAsync(user, role.Name);
                identityResult.CheckError();
            }

        }
    }
}
