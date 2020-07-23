using Company.Project.Authorization;
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
        #region IRivenDbContext 属性实现

        [NotMapped]
        public virtual bool AuditSuppressAutoSetTenantName => true;

        [NotMapped]
        public virtual IServiceProvider ServiceProvider { get; }

        [NotMapped]
        public virtual ConcurrentDictionary<Type, object> SerivceInstanceMap => new ConcurrentDictionary<Type, object>();

        [NotMapped]
        public virtual IRivenDbContext Self => this;

        #endregion

        #region AppSession 实例


        [NotMapped]
        public virtual IAppSession AppSession => Self.GetApplicationService<IAppSession>();


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options)
        {
            ServiceProvider = serviceProvider;
        }

        #region 租户

        public DbSet<Tenant> Tenants { get; set; }

        #endregion


        #region 示例实体

        public DbSet<SampleEntity> SampleEntitys { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureGlobalFilters(this);

            modelBuilder.ConfiurationIdentityTables();

            modelBuilder.ConfiurationTenantTable();
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

        #region IRivenDbContext 接口函数实现

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

        #endregion
    }
}
