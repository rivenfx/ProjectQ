using Riven.Entities;
using Riven.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class RoleClaim : AppRoleClaim<long>, IEntity<int>
    {
        public bool EntityEquals(object obj)
        {
            return EntityHelper.EntityEquals(this, obj);
        }

        public bool IsTransient()
        {
            return EntityHelper.IsTransient(this);
        }
    }
}
