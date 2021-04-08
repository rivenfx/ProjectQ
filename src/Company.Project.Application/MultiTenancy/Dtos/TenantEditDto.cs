using Riven.Dtos;
using Riven.Entities;
using Riven.Entities.Auditing;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy.Dtos
{
    public class TenantEditDto
    {
        public TenantDto EntityDto { get; set; }

    }
}
