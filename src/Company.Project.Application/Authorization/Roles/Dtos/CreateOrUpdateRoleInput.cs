using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles.Dtos
{
    public class CreateOrUpdateRoleInput
    {
        public RoleEditDto EntityDto { get; set; }

        public List<string> Claims { get; set; }
    }
}
