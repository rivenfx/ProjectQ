using Riven.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users.Dtos
{
    public class UserDto : EntityDto<Guid?>
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 电话号码是否已确认
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 邮箱是否已确认
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// 启用锁定
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// 是否已激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否启用双重校验
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }
    }
}
