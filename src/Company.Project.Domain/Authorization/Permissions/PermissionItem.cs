using JetBrains.Annotations;

using Microsoft.AspNetCore.Components.Forms;

using Riven;
using Riven.Identity.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public class PermissionItem
    {
        static int _sort = 0;

        public virtual int HashCode { get; protected set; }

        public virtual string Parent { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual PermissionAuthorizeScope Scope { get; protected set; }

        public virtual int Sort { get; protected set; }



        public PermissionItem([NotNull] string name, string parent = null, PermissionAuthorizeScope scope = PermissionAuthorizeScope.Common)
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

            if (obj is PermissionItem input)
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
