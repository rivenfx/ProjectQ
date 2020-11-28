

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Riven;
using Riven.Repositories;
using Riven.Uow;

namespace Company.Project.MultiTenancy
{
    public class TenantMananger : DomainService<Tenant, Guid>, ITenantManager
    {
        public TenantMananger(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task<Tenant> Create(string name, string displayName, string description = null, string connectionString = null, bool isStatic = false)
        {
            var tenant = new Tenant()
            {
                Name = name,
                DisplayName = displayName,
                Description = description,
                ConnectionString = connectionString,
                IsStatic = isStatic
            };

            await this.Create(tenant);

            return tenant;
        }

        public virtual async Task Delete(string name)
        {
            await this.Delete(o => o.Name == name);
        }

        public virtual async Task<Tenant> GetByName(string name)
        {
            return await this.QueryAsNoTracking.FirstOrDefaultAsync(o => o.Name == name);
        }

        public virtual async Task<Tenant> Update(string name, string displayName, string description)
        {
            var tenant = await this.GetByName(name);
            if (tenant == null)
            {
                return null;
            }
            tenant.DisplayName = displayName;
            tenant.Description = description;

            await this.Update(tenant);

            return tenant;
        }
    }
}
