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

        public virtual int HashCode { get; protected set; }

        public virtual string Parent { get; protected set; }

        public virtual string Claim { get; protected set; }

        public virtual ClaimItemType Type { get; protected set; }

        public virtual int Sort { get; protected set; }



        public ClaimItem([NotNull] string claim, ClaimItemType type, string parent = null)
        {
            Check.NotNull(claim, nameof(claim));

            this.Claim = claim;
            this.Type = type;
            this.Parent = parent;

            this.Sort = _sort++;

            this.HashCode = this.Claim.GetHashCode() + this.Type.GetHashCode();
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

            if (obj is ClaimItem input)
            {
                return input.GetHashCode() == this.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.HashCode;
        }
    }
}
