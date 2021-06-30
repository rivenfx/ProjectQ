using Company.Project.Authorization;
using Company.Project.Authorization.Permissions;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Database.Extenstions;
using Company.Project.MultiTenancy;
using Company.Project.Samples;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Riven;
using Riven.Database;
using Riven.Identity;
using Riven.Identity.Roles;
using Riven.Identity.Users;
using Riven.MultiTenancy;

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
        : RivenDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {

        }


        #region 权限

        /// <summary>
        /// 权限
        /// </summary>
        public virtual DbSet<Permission> Permissions { get; set; }

        #endregion


        #region 租户

        public DbSet<Tenant> Tenants { get; set; }

        #endregion


        #region 示例实体

        public DbSet<SampleEntity> SampleEntitys { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 启用 Filter
            modelBuilder.ConfigureGlobalFilters(this);

            modelBuilder.ConfiurationIdentityTables();

            modelBuilder.ConfiurationTenantTable();

            modelBuilder.ConfiurationRivenListViewInfo();
        }

        #region 重写SaveChange函数

        public override int SaveChanges()
        {
            this.ApplyAudit(ChangeTracker);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAudit(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ApplyAudit(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAudit(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion
    }
}