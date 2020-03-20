using Company.Project.Authorization.Users;
using Riven.Repositories;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Company.Project.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Company.Project.SeedData
{
    public interface ISeeder
    {
        void Create();
    }

    public class Seeder : ISeeder
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
            var userRepo = _serviceProvider.GetService<IRepository<User>>();
            var adminUser = userRepo.GetAll().FirstOrDefault(o => o.UserName == "admin");
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

                adminUser.NormalizedUserName = adminUser.UserName.ToLower();
                adminUser.NormalizedEmail = adminUser.Email.ToLower();


                adminUser.PasswordHash = this._passwordHasher.HashPassword(adminUser, "123qwe");
                userRepo.InsertAsync(adminUser);
            }

        }
    }
}
