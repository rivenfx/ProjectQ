

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Riven;
using Riven.Repositories;
using Riven.Uow;
using Riven.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace Company.Project.MultiTenancy
{
    public class TenantMananger : DomainService<Tenant, Guid>, ITenantManager
    {
        public TenantMananger(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task<Tenant> Create(string name, string displayName, string description = null, string connectionString = null, bool isStatic = false, bool isActive = false)
        {
            var tenant = new Tenant()
            {
                Name = name,
                DisplayName = displayName,
                Description = description,
                ConnectionString = connectionString,
                IsStatic = isStatic,
                IsActive = isActive
            };

            await this.Create(tenant, true);

            // 如果使用数据库链接字符串,那么将数据库链接字符串加入访问器中
            if (!connectionString.IsNullOrWhiteSpace())
            {
                var connectionStringStorage = this._serviceProvider.GetService<IConnectionStringStorage>();
                connectionStringStorage.AddOrUpdate(
                    new ConnectionStringProvider(tenant.Name, tenant.ConnectionString)
                    );
            }

            return tenant;
        }

        public virtual async Task Delete(string name)
        {
            var tenant = await this.GetByName(name);
            if (tenant == null)
            {
                return;
            }

            // 清除连接字符串
            if (!tenant.ConnectionString.IsNullOrWhiteSpace())
            {
                var connectionStringStorage = this._serviceProvider.GetService<IConnectionStringStorage>();
                connectionStringStorage.Remove(tenant.Name);
            }

            await this.Delete(tenant.Id);
        }

        public virtual async Task<Tenant> GetByName(string name)
        {
            return await this.QueryAsNoTracking.FirstOrDefaultAsync(o => o.Name == name);
        }

        public virtual async Task<string> GetDisplayNameByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }


            return await this.QueryAsNoTracking
                .Where(o => o.Name == name)
                .Select(o => o.DisplayName)
                .FirstOrDefaultAsync();
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

        public virtual IEnumerable<IConnectionStringProvider> LoadConnectionStringProviders()
        {
            var queryResult = this.QueryAsNoTracking.Where(o => o.ConnectionString != null)
                 .Select(o => new
                 {
                     Name = o.Name,
                     ConnStr = o.ConnectionString
                 })
                 .ToList();

            return queryResult.Where(o => !o.ConnStr.IsNullOrWhiteSpace())
                 .Select(o => new ConnectionStringProvider(o.Name, o.ConnStr));
        }
    }
}
