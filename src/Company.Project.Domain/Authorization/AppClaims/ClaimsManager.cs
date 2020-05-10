using System.Collections.Generic;
using System.Linq;
using Riven.Dependency;

namespace Company.Project.Authorization.AppClaims
{
    public class ClaimsManager : IClaimsManager, ISingletonDependency
    {
        static List<string> _claims = new List<string>();

        public void Add(params string[] claims)
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

        public IQueryable<string> GetAll()
        {
            return _claims.AsQueryable();
        }

        public void Remove(params string[] claims)
        {
            if (claims == null || claims.Length == 0)
            {
                return;
            }
            _claims = _claims.Except(claims).ToList();
        }
    }
}
