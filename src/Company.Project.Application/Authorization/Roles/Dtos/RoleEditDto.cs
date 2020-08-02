using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles.Dtos
{
    public class RoleEditDto
    {
        public RoleDto EntityDto { get; set; }

        public List<string> Claims { get; set; }
    }
}
