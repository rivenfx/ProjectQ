using JetBrains.Annotations;

using Microsoft.AspNetCore.Components.Forms;

using Riven;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    public class ClaimItem
    {
        static int _sort = 0;

        public string Parent { get; private set; }

        public string Claim { get; private set; }

        public ClaimItemType Type { get; private set; }

        public int Sort { get; private set; }



        public ClaimItem([NotNull] string claim, ClaimItemType type, string parent = null)
        {
            Check.NotNull(claim, nameof(claim));

            this.Claim = claim;
            this.Type = type;
            this.Parent = parent;

            this.Sort = _sort++;
        }


        public static ClaimItem CreateWithCommon(string claim, string parent = null)
        {
            return new ClaimItem(claim, ClaimItemType.Common, parent);
        }

        public static ClaimItem CreateWithHost(string claim, string parent = null)
        {
            return new ClaimItem(claim, ClaimItemType.Host, parent);
        }

        public static ClaimItem CreateWithTenant(string claim, string parent = null)
        {
            return new ClaimItem(claim, ClaimItemType.Tenant, parent);
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

        public override int GetHashCode()
        {
            return this.Claim.GetHashCode() + this.Type.GetHashCode();
        }
    }
}
