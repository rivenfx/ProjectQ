
using System;
using System.Threading.Tasks;
using Company.Project.Database;
using Microsoft.EntityFrameworkCore;
using Company.Project.MultiTenancy;

namespace Company.Project.Seeder
{
    public class TenantSeeder : SeederBase, ITenantSeeder
    {
        public TenantSeeder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task Create(DbContext dbContext, string tenantName)
        {
            if (!(dbContext is AppDbContext))
            {
                throw new Exception("The database context type is incorrect!");
            }


            var defaultRole = await this.CreateRoles(dbContext, tenantName);

            var defaultUser = await this.CreateUsers(dbContext, defaultRole);
        }

    }
}

