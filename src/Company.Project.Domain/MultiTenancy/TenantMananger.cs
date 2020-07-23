

using System;
using System.Linq;

using Riven.Repositories;

namespace Company.Project.MultiTenancy
{
    public class TenantMananger : ITenantManager
    {
        readonly IRepository<Tenant, Guid> _entityRep;

        public TenantMananger(IRepository<Tenant, Guid> entityRep)
        {
            _entityRep = entityRep;
        }

        public IQueryable<Tenant> Query => _entityRep.GetAll();

        public IQueryable<Tenant> QueryAsNoTracking => this.Query.AsNoTracking();

    }
}
