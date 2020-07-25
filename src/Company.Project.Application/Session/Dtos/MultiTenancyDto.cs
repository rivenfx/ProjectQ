using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Session.Dtos
{
    public class MultiTenancyDto
    {
        public bool IsEnabled { get; set; }

        public string TenantName { get; set; }
    }
}
