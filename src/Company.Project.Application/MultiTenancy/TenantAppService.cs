using Company.Project.MultiTenancy.Dtos;

using Riven.Application;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.MultiTenancy
{
    public class TenantAppService : IApplicationService
    {
        readonly ITenantManager _tenantManager;

        public TenantAppService(ITenantManager tenantManager)
        {
            _tenantManager = tenantManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await _tenantManager.GetByName(input.TenantName);

            var result = new IsTenantAvailableOutput();
            result.TenantName = tenant?.Name;

            if (tenant == null)
            {
                result.State = TenantAvailabilityState.NotFound;
            }
            else
            {
                result.State = tenant.IsActive ? TenantAvailabilityState.Available : TenantAvailabilityState.InActive;
            }

            return result;
        }

    }
}
