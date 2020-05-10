using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    public interface IClaimsManager
    {
        IQueryable<string> GetAll();


        void Add(params string[] claims);


        void Remove(params string[] claims);


        void Clear();
    }
}
