using Company.Project.Authorization;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Samples;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Riven;
using Riven.Identity.Roles;
using Riven.Identity.Users;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContext
        : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IRivenDbContext,
        IRivenFilterDbContext,
        IRivenAuditedDbContext
    {
        [NotMapped]
        public IServiceProvider ServiceProvider { get; }

        [NotMapped]
        public ConcurrentDictionary<Type, object> SerivceInstanceMap { get; } = new ConcurrentDictionary<Type, object>();

        [NotMapped]
        public IAppSession AppSession => ServiceProvider?.GetService<IAppSession>();

        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options)
        {
            ServiceProvider = serviceProvider;
        }

        public DbSet<SampleEntity> SampleEntitys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurationIdentityTables(modelBuilder);
        }


        /// <summary>
        /// 配置Identity的表
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        protected virtual ModelBuilder ConfigurationIdentityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(Role)}s");
            });
            modelBuilder.Entity<RoleClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(RoleClaim)}s");
            });

            modelBuilder.Entity<User>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(User)}s");
                entityBuilder.HasIndex(o => o.Nickname)
                        .IsUnique();
            });
            modelBuilder.Entity<UserClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserClaim)}s");
            });
            modelBuilder.Entity<UserLogin>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserLogin)}s");
            });
            modelBuilder.Entity<UserToken>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserToken)}s");
            });
            modelBuilder.Entity<UserRole>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserRole)}s");
            });

            return modelBuilder;
        }


        public bool GetMultiTenancyEnabled()
        {
            return true;
        }

        public string GetCurrentTenantNameOrNull()
        {
            return AppSession?.TenantIdString;
        }

        public string GetCurrentUserIdOrNull()
        {
            return AppSession?.UserIdString;
        }
    }
}
