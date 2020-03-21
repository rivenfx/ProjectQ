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
        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly UserPasswordHasher _passwordHasher;


        public Seeder(IServiceProvider serviceProvider, IUnitOfWorkManager unitOfWorkManager, UserPasswordHasher passwordHasher)
        {
            _serviceProvider = serviceProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _passwordHasher = passwordHasher;
        }

        public void Create()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    this.CreateUsers();


                    uow.Complete();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
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

            //    var userRepo = _serviceProvider.GetService<IRepository<User>>();
            //var adminUser = userRepo.GetAll().FirstOrDefault(o => o.UserName == "admin");
            //if (adminUser == null)
            //{
            //    adminUser = new User();

            //    adminUser.UserName = "admin";
            //    adminUser.PhoneNumber = "13028166007";
            //    adminUser.PhoneNumberConfirmed = true;
            //    adminUser.Email = "yi.hang@live.com";
            //    adminUser.EmailConfirmed = true;
            //    adminUser.LockoutEnabled = false;
            //    adminUser.TwoFactorEnabled = false;
            //    adminUser.IsActive = true;

            //    adminUser.NormalizedUserName = adminUser.UserName.ToLower();
            //    adminUser.NormalizedEmail = adminUser.Email.ToLower();


            //    adminUser.PasswordHash = this._passwordHasher.HashPassword(adminUser, "123qwe");
            //    userRepo.InsertAsync(adminUser);
            //}

        }
    }
}
