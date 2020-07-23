using Company.Project.Authorization.Users;
using Riven.Uow;
using System;
using Microsoft.Extensions.DependencyInjection;
using Company.Project.Authorization;
using Riven.Dependency;
using Company.Project.Authorization.Roles;
using Riven.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Company.Project.SeedData
{
    public class Seeder : ISeeder, ITransientDependency
    {
        protected readonly IServiceProvider _serviceProvider;


        public Seeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Create()
        {

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWorkManager = scope.ServiceProvider.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                       

                        await uow.CompleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        protected virtual Task Create(IServiceProvider scopeServiceProvider)
        {

        }

        /// <summary>
        /// 创建管理员用户和角色
        /// </summary>
        /// <param name="scopeServiceProvider"></param>
        /// <returns></returns>
        protected async Task CreateAdminUserAndRole(IServiceProvider scopeServiceProvider)
        {
            // 管理员角色
            var roleManager = scopeServiceProvider.GetService<RoleManager>();
            var systemRole = await roleManager.FindByNameAsync(AppConsts.Authorization.SystemRoleName);
            if (systemRole == null)
            {
                systemRole = await roleManager.CreateAsync(
                        AppConsts.Authorization.SystemRoleName,
                        AppConsts.Authorization.SystemRoleDisplayName,
                        AppConsts.Authorization.SystemRoleDisplayName
                    );
            }

            // 附加权限
            await roleManager.ChangeIdentityClaimsAsync(systemRole,
                          AppClaimsConsts.User.Query,
                          AppClaimsConsts.User.Create,
                          AppClaimsConsts.User.Edit,
                          AppClaimsConsts.User.Delete,

                          AppClaimsConsts.Role.Query,
                          AppClaimsConsts.Role.Edit,
                          AppClaimsConsts.Role.Delete
                          );

            var userManager = scopeServiceProvider.GetService<UserManager>();
            // 管理员用户
            var systemUser = await userManager.FindByNameOrEmailOrPhoneNumberAsync(AppConsts.Authorization.SystemUserName);
            if (systemUser == null)
            {
                systemUser = await userManager.CreateAsync(
                        AppConsts.Authorization.SystemUserName,
                        AppConsts.Authorization.SystemUserPassword,
                        AppConsts.Authorization.SystemUserName,
                        "13000000007",
                        true,
                        "msmadaoe@msn.com",
                        true,
                        false,
                        true
                    );
            }

            // 用户添加角色
            var roles = await userManager.GetRolesAsync(systemUser);
            if (!roles.Any(o => o == systemRole.Name))
            {
                await userManager.AddToRolesAsync(systemUser, systemRole.Name);
            }


        }
    }
}
