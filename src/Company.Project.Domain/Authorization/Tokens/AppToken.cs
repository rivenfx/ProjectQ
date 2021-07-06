using AspNetCore.Authentication.ApiToken;

using Riven.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Project.Authorization.Tokens
{
    public class AppToken : TokenModel, IEntity<long>, IMayHaveTenant
    {
        public virtual long Id { get; set; }

        public virtual string TenantName { get; set; }

        public virtual bool EntityEquals(object obj)
        {
            return EntityHelper.EntityEquals(this, obj);
        }

        public virtual bool IsTransient()
        {
            return EntityHelper.IsTransient(this);
        }
    }

    public class ClaimLite
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }

    }
}
