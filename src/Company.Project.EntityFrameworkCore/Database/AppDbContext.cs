using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Samples;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riven;
using Riven.Identity.Roles;
using Riven.Identity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContext
        : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {

        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<SampleEntity> SampleEntitys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigurationIdentityTables(builder);
        }


        /// <summary>
        /// 配置Identity的表
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual ModelBuilder ConfigurationIdentityTables(ModelBuilder builder)
        {
            builder.Entity<Role>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(Role)}s");
            });
            builder.Entity<RoleClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(RoleClaim)}s");
            });

            builder.Entity<User>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(User)}s");
            });
            builder.Entity<UserClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserClaim)}s");
            });
            builder.Entity<UserLogin>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserLogin)}s");
            });
            builder.Entity<UserToken>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserToken)}s");
            });
            builder.Entity<UserRole>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserRole)}s");
            });

            return builder;
        }
    }
}
