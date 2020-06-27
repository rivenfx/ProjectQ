using Company.Project.Authorization.AppClaims;

using Microsoft.EntityFrameworkCore;

using Riven.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Claims
{
    public class ClaimsAppService : IApplicationService
    {
        readonly IClaimsManager _claimsManager;

        public ClaimsAppService(IClaimsManager claimsManager)
        {
            _claimsManager = claimsManager;
        }

        public async Task<List<string>> GetAllClaims()
        {
            return await Task.FromResult(
                _claimsManager.GetAll().ToList()
                );
        }

    }
}
