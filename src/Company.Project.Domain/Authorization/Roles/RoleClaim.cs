using Microsoft.AspNetCore.Identity;

using Riven.Entities;
using Riven.Entities.Auditing;
using Riven.Identity.Roles;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class RoleClaim : IdentityRoleClaim<Guid>, IEntity<int>, IMayHaveTenant
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
