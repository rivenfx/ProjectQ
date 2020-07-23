using Riven.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy
{
    public class Tenant : Entity<Guid>
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

    }
}
