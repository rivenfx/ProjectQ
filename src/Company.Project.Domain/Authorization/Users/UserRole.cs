using Microsoft.AspNetCore.Identity;

using Riven.Entities;
using Riven.Entities.Auditing;
using Riven.Identity.Users;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class UserRole : IdentityUserRole<Guid>, IEntity<Guid>, IMayHaveTenant
    {
        public virtual Guid Id { get; set; }

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
