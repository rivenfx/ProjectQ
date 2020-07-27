using System.Collections.Generic;
using System.Linq;

using Riven.Dependency;

namespace Company.Project.Authorization.AppClaims
{
    public class ClaimsManager : IClaimsManager, ISingletonDependency
    {
        static List<ClaimItem> _claims = new List<ClaimItem>();

        public void Add(params ClaimItem[] claims)
        {
            if (claims == null || claims.Length == 0)
            {
                return;
            }
            _claims = _claims.Union(claims).Distinct().ToList();
        }

        public void Clear()
        {
            _claims.Clear();
        }

        public IQueryable<ClaimItem> GetAll(ClaimItemType claimItemType)
        {
            // 如果筛选类型为 Host 但是没有开启多租户,那么修改类型为 Tenant
            if (claimItemType == ClaimItemType.Host && !Riven.MultiTenancy.MultiTenancyConfig.IsEnabled)
            {
                claimItemType = ClaimItemType.Tenant;
            }

            return _claims.Where(o => o.Type == ClaimItemType.Common || o.Type == claimItemType)
                .OrderBy(o => o.Sort)
                .AsQueryable();
        }

        public void Remove(params ClaimItem[] claims)
        {
            if (claims == null || claims.Length == 0)
            {
                return;
            }
            _claims = _claims.Except(claims).ToList();
        }
    }
}
