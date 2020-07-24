

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Riven;
using Riven.Repositories;
using Riven.Uow;

namespace Company.Project.MultiTenancy
{
    public class TenantMananger : ITenantManager
    {
        readonly IRepository<Tenant, Guid> _entityRep;
        readonly IUnitOfWorkManager _unitOfWorkManager;

        IActiveUnitOfWork CurrentUnitOfWork => this._unitOfWorkManager.Current;

        public TenantMananger(IRepository<Tenant, Guid> entityRep, IUnitOfWorkManager unitOfWorkManager)
        {
            _entityRep = entityRep;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public IQueryable<Tenant> Query => _entityRep.GetAll();

        public IQueryable<Tenant> QueryAsNoTracking => this.Query.AsNoTracking();


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

            return await _entityRep.InsertAsync(tenant);
        }

        public virtual async Task Delete(string name)
        {
            await _entityRep.DeleteAsync(o => o.Name == name);
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

            return await this._entityRep.UpdateAsync(tenant);
        }
    }
}
