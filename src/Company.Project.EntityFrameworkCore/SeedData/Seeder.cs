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
                        await this.CreateAdminUserAndRole(scope.ServiceProvider);

                        await uow.CompleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


        protected async Task CreateAdminUserAndRole(IServiceProvider scopeServiceProvider)
        {
            // ����Ա��ɫ
            var roleManager = scopeServiceProvider.GetService<RoleManager>();
            var adminRole = await roleManager.FindByNameAsync("admin");
            if (adminRole == null)
            {
                adminRole = await roleManager.CreateAsync(
                        "admin",
                        "Administrator",
                        "Ӧ�ó���ϵͳ����Ա",

                        AppClaimsConsts.User.Create,
                        AppClaimsConsts.User.Edit,
                        AppClaimsConsts.User.Delete,

                        AppClaimsConsts.Role.Create,
                        AppClaimsConsts.Role.Edit,
                        AppClaimsConsts.Role.Delete
                    );
            }

            var userManager = scopeServiceProvider.GetService<UserManager>();
            // ����Ա�û�
            var adminUser = await userManager.FindByNameOrEmailOrPhoneNumberAsync("admin");
            if (adminUser == null)
            {
                adminUser = await userManager.CreateAsync(
                        "admin",
                        "123qwe",
                        "����Ա",
                        "13028166007",
                        true,
                        "yi.hang@live.com",
                        true,
                        false,
                        true
                    );
            }

            // �û���ӽ�ɫ
            var roles = await userManager.GetRolesAsync(adminUser);
            if (!roles.Any(o => o == adminRole.Name))
            {
                await userManager.AddToRolesAsync(adminUser, adminRole.Name);
            }


        }
    }
}
