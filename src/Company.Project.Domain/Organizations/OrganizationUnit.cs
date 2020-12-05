using Riven.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Organizations
{
    /// <summary>
    /// 组织单位实体
    /// </summary>
    public class OrganizationUnit : Entity<Guid>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
    }
}
