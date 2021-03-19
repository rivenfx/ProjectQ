using Riven.Identity.Permissions;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public class PermissionManager : PermissionManager<Permission>
    {
        public PermissionManager(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }
    }
}
