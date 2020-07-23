using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Riven.Dependency;
using Riven.Repositories;

namespace Company.Project.MultiTenancy
{
    public interface ITenantManager : ITransientDependency
    {
        IQueryable<Tenant> Query { get; }
        IQueryable<Tenant> QueryAsNoTracking { get; }
    }
}
