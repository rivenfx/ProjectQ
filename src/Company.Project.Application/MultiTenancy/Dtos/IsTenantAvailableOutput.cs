using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy.Dtos
{
    public class IsTenantAvailableOutput
    {
        public TenantAvailabilityState State { get; set; }

        public string TenantName { get; set; }
    }

   
}
