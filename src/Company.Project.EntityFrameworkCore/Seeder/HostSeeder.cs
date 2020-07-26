
using System;
using System.Threading.Tasks;
using System.Linq;
using Company.Project.Database;
using Microsoft.EntityFrameworkCore;
using Company.Project.MultiTenancy;

namespace Company.Project.Seeder
{
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

            var tenant = await tenantStore.IgnoreQueryFilters()
                .Where(o => o.Name == tenantName)
                .FirstOrDefaultAsync();
            if (tenant == null)
            {
                tenant = new Tenant()
                {
                    Name = tenantName,
                    DisplayName = tenantName,
                    Description = tenantName,
                    IsStatic = true,
                    IsActive = true
                };
                await tenantStore.AddAsync(tenant);
                await dbContext.SaveChangesAsync();
            }

            return tenant;
        }
    }
}