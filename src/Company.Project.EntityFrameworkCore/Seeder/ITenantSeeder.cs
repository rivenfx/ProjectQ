using Company.Project.Authorization.Users;

using Riven.Dependency;
using Riven.Uow;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Company.Project.Authorization.Roles;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Company.Project.MultiTenancy;

namespace Company.Project.Seeder
{
    /// <summary>
    /// 租户种子数据
    /// </summary>
    public interface ITenantSeeder : IScopeDependency
    {
        Task Create(DbContext dbContext, string tenantName);
    }
}

