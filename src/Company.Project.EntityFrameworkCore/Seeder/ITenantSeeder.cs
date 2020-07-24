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
    /// 默认租户种子数据
    /// </summary>
    public interface ITenantSeeder : IScopeDependency
    {
        Task Create(DbContext dbContext, Tenant tenant);
    }

    public class TenantSeeder : SeederBase, ITenantSeeder
    {
        public TenantSeeder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task Create(DbContext dbContext, Tenant tenant)
        {
            if (!(dbContext is AppDbContext))
            {
                throw new Exception("The database context type is incorrect!");
            }


            var defaultRole = await this.CreateRoles(dbContext, tenant.Name);

            var defaultUser = await this.CreateUsers(dbContext, defaultRole);
        }

    }
}

