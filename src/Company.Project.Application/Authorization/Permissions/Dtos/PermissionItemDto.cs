using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions.Dtos
{
    public class PermissionItemDto
    {
        public string Parent { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }
}
