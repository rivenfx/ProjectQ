using Company.Project.Authorization.AppClaims;

using Microsoft.EntityFrameworkCore;

using Riven.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riven;
using Riven.Extensions;
using Company.Project.Authorization.Claims.Dtos;
using Riven.Identity.Authorization;

namespace Company.Project.Authorization.Claims
{
    [ClaimsAuthorize]
    public class ClaimsAppService : IApplicationService
    {
        readonly IAppSession _appSession;
        readonly IClaimsManager _claimsManager;

        public ClaimsAppService(IAppSession appSession, IClaimsManager claimsManager)
        {
            _appSession = appSession;
            _claimsManager = claimsManager;
        }

        public async Task<List<string>> GetAllClaims()
        {
            var claimItemType = _appSession.TenantName.IsNullOrWhiteSpace() ? ClaimItemType.Host : ClaimItemType.Tenant;

            return await Task.FromResult(
                _claimsManager.GetAll(claimItemType).Select(o => o.Claim).ToList()
                );
        }

        public async Task<List<ClaimItemDto>> GetAllClaimsWithTree()
        {
            var claimItemType = _appSession.TenantName.IsNullOrWhiteSpace() ? ClaimItemType.Host : ClaimItemType.Tenant;

            return await Task.FromResult(
                _claimsManager.GetAll(claimItemType).ProjectTo<ClaimItemDto>().ToList()
                );
        }

    }
}
