using Riven.Entities;
using Riven.Entities.Auditing;
using Riven.Identity.Roles;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class RolePermission : AppRolePermission<Guid>, IEntity<int>, IFullAudited, IMayHaveTenant
    {
        public virtual string Creator { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual string Deleter { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string TenantName { get; set; }

        public virtual bool EntityEquals(object obj)
        {
            return EntityHelper.EntityEquals(this, obj);
        }

        public virtual bool IsTransient()
        {
            return EntityHelper.IsTransient(this);
        }
    }
}
