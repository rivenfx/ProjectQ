using Riven.Identity.Permissions;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public class PermissionStore : IdentityPermissionStore<Permission>
    {
        public PermissionStore(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
