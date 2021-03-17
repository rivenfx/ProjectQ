using Riven.Entities;
using Riven.Identity.Permissions;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public class Permission : AppPermission, IEntity<Guid>, IMayHaveTenant
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
