using Company.Project.Authorization.Users;
using Riven.Uow;
using System;
using Microsoft.Extensions.DependencyInjection;
using Company.Project.Authorization;
using Riven.Dependency;

namespace Company.Project.SeedData
{
    public class Seeder : ISeeder, ITransientDependency
    {
        protected readonly IServiceProvider _serviceProvider;


        public Seeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Create()
        {
            this.CreateUsers();
        }


        protected void CreateUsers()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager>();
                var adminUser = userManager.FindByNameOrEmailOrPhoneNumberAsync("admin").Result;
                if (adminUser == null)
                {
                    adminUser = new User();

                    adminUser.UserName = "admin";
                    adminUser.PhoneNumber = "13028166007";
                    adminUser.PhoneNumberConfirmed = true;
                    adminUser.Email = "yi.hang@live.com";
                    adminUser.EmailConfirmed = true;
                    adminUser.LockoutEnabled = false;
                    adminUser.TwoFactorEnabled = false;
                    adminUser.IsActive = true;

                    var identityResult = userManager.CreateAsync(adminUser, "123qwe").Result;
                }
            }
        }
    }
}
