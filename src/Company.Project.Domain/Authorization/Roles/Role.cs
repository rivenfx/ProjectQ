using Riven.Entities;
using Riven.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class Role : AppRole<long>, IEntity<long>
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
