using Riven.Entities;
using Riven.Entities.Auditing;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy
{
    public class Tenant : Entity<Guid>, IFullAudited, IPassivable
    {
        /// <summary>
        /// 租户名称 唯一
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 租户显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// 是否为内置
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// 是否已激活
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual string Creator { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual string Deleter { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }

    }
}
