using JetBrains.Annotations;

using Microsoft.AspNetCore.Components.Forms;

using Riven;
using Riven.Identity.Authorization;
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

        public virtual ClaimsAuthorizeScope Scope { get; protected set; }

        public virtual int Sort { get; protected set; }



        public ClaimItem([NotNull] string claim, ClaimsAuthorizeScope scope, string parent = null)
        {
            Check.NotNull(claim, nameof(claim));

            this.Claim = claim;
            this.Scope = scope;
            this.Parent = parent;

            this.Sort = _sort++;

            this.HashCode = this.Claim.GetHashCode() + this.Scope.GetHashCode();
        }


        public static ClaimItem CreateWithCommon(string claim, string parent = null)
        {
            return new ClaimItem(claim, ClaimsAuthorizeScope.Common, parent);
        }

        public static ClaimItem CreateWithHost(string claim, string parent = null)
        {
            return new ClaimItem(claim, ClaimsAuthorizeScope.Host, parent);
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
