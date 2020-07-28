using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    public interface IClaimsManager
    {
        IQueryable<ClaimItem> GetAll(ClaimItemType claimItemType);


        void Add(params ClaimItem[] claims);


        void Remove(params ClaimItem[] claims);


        void Clear();
    }
}
