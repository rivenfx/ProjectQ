using Riven.Entities;
using Riven.Entities.Auditing;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy
{
    public class Tenant : FullAuditedEntity<Guid>, IFullAudited, IPassivable
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

    }
}
