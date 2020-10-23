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
            await Task.Yield();


            return _claimsManager.GetAll().Select(o => o.Name).ToList();
        }

        public async Task<List<ClaimItemDto>> GetAllClaimsWithTree()
        {
            await Task.Yield();


            return _claimsManager.GetAll().ProjectTo<ClaimItemDto>().ToList();
        }

    }
}
