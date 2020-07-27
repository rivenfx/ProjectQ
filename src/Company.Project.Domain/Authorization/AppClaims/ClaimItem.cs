using JetBrains.Annotations;

using Riven;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    public class ClaimItem
    {
        public string Claim { get; private set; }

        public ClaimItemType Type { get; private set; }


        public ClaimItem([NotNull] string claim, ClaimItemType type)
        {
            Check.NotNull(claim, nameof(claim));
            Claim = claim;
            Type = type;
        }


        public static ClaimItem CreateWithCommon(string claim)
        {
            return new ClaimItem(claim, ClaimItemType.Common);
        }

        public static ClaimItem CreateWithHost(string claim)
        {
            return new ClaimItem(claim, ClaimItemType.Host);
        }

        public static ClaimItem CreateWithTenant(string claim)
        {
            return new ClaimItem(claim, ClaimItemType.Tenant);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var input = obj as ClaimItem;

            return this.Claim == input.Claim && this.Type == input.Type;
        }
    }
}
