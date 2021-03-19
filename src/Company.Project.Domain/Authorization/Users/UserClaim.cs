using Microsoft.AspNetCore.Identity;

using Riven.Entities;
using Riven.Entities.Auditing;
using Riven.Identity.Users;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class UserClaim : IdentityUserClaim<Guid>, IEntity<int>, IMayHaveTenant
    {
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
}
