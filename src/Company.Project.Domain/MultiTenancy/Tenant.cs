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
        /// �⻧���� Ψһ
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// �⻧��ʾ����
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// �Ƿ�Ϊ����
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// �Ƿ��Ѽ���
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
