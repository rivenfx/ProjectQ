using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Roles;
using Riven.Uow;

using Company.Project.Authorization.Users;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Linq;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore : IdentityRoleStore<Role, Guid, UserRole, RoleClaim>
    {
        public RoleStore(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
