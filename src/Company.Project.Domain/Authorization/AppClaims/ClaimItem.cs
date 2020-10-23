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

        public virtual string Name { get; protected set; }

        public virtual ClaimsAuthorizeScope Scope { get; protected set; }

        public virtual int Sort { get; protected set; }



        public ClaimItem([NotNull] string name, string parent = null, ClaimsAuthorizeScope scope = ClaimsAuthorizeScope.Common)
        {
            Check.NotNull(name, nameof(name));

            this.Name = name;
            this.Scope = scope;
            this.Parent = parent;

            this.Sort = _sort++;

            this.HashCode = this.Name.GetHashCode() + this.Scope.GetHashCode();
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
