using Company.Project.Authorization.Users;

using Riven.Dependency;
using Riven.Uow;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Company.Project.Authorization.Roles;
using System.Linq;
using Company.Project.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Company.Project.MultiTenancy;

namespace Company.Project.Seeder
{
    /// <summary>
    /// Host 种子数据
    /// </summary>
    public interface IHostSeeder : IScopeDependency
    {
        Task<Tenant> Create(DbContext dbContext);
    }

    public class HostSeeder : SeederBase, IHostSeeder
    {
        public HostSeeder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task<Tenant> Create(DbContext dbContext)
        {
            if (!(dbContext is AppDbContext))
            {
                throw new Exception("The database context type is incorrect!");
            }


            var defaultRole = await this.CreateRoles(dbContext, null);

            var defaultUser = await this.CreateUsers(dbContext, defaultRole);

            var defaultTenant = await this.CreateDefaultTenant(dbContext, AppConsts.MultiTenancy.DefaultTenantName);

            return defaultTenant;
        }


        /// <summary>
        /// 创建默认租户
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public virtual async Task<Tenant> CreateDefaultTenant(DbContext dbContext, string tenantName)
        {
            var tenantStore = dbContext.Set<Tenant>();

            var tenant = await tenantStore.AsQueryable().IgnoreQueryFilters()
                .Where(o => o.Name == tenantName)
                .FirstOrDefaultAsync();
            if (tenant == null)
            {
                tenant = new Tenant()
                {
                    Name = tenantName,
                    DisplayName = tenantName,
                    Description = tenantName
                };
                await tenantStore.AddAsync(tenant);
                await dbContext.SaveChangesAsync();
            }

            return tenant;
        }
    }
}