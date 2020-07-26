using Company.Project.MultiTenancy.Dtos;

using Riven.Application;
using Riven.Uow;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.MultiTenancy
{
    public class TenantAppService : IApplicationService
    {
        readonly IUnitOfWorkManager _unitOfWorkManager;
        readonly ITenantManager _tenantManager;

        public TenantAppService(IUnitOfWorkManager unitOfWorkManager, ITenantManager tenantManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _tenantManager = tenantManager;
        }

        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            using (var uow = _unitOfWorkManager.Begin())
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
}
