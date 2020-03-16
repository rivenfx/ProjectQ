using Riven.Entities;
using Riven.Identity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class UserClaim : AppUserClaim<long>, IEntity<int>
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
