using Company.Project.Authorization.AppClaims;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Claims.Dtos
{
    public class ClaimItemDto
    {
        public string Parent { get; set; }

        public string Claim { get; set; }

        public int Sort { get; set; }
    }
}
