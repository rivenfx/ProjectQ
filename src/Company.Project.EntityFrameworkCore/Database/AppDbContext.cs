using Company.Project.Authorization;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Samples;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
using System.Threading;
using System.Threading.Tasks;

namespace Company.Project.Database
{
    public class AppDbContext
        : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IRivenDbContext
    {
        #region 功能实例

        [NotMapped]
        public virtual IServiceProvider ServiceProvider { get; }

        [NotMapped]
        public virtual ConcurrentDictionary<Type, object> SerivceInstanceMap => new ConcurrentDictionary<Type, object>();

        [NotMapped]
        public virtual IAppSession AppSession => Self.GetApplicationService<IAppSession>();

        [NotMapped]
        public virtual IRivenDbContext Self => this;

        #endregion

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

        #region 重写SaveChange函数

        public override int SaveChanges()
        {
            this.Self.ApplyAudit(ChangeTracker);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.Self.ApplyAudit(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.Self.ApplyAudit(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.Self.ApplyAudit(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion


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

        public virtual string GetCurrentTenantNameOrNull()
        {
            return AppSession?.TenantIdString;
        }

        public virtual string GetCurrentUserIdOrNull()
        {
            return AppSession?.UserIdString;
        }

        public virtual EntityEntry ConvertToEntry(object obj)
        {
            return Entry(obj);
        }
    }
}
