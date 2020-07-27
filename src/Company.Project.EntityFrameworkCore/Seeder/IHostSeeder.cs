using Company.Project.Authorization.Users;

using Riven.Dependency;
using Riven.Uow;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Company.Project.Authorization.Roles;
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
}