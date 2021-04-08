using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.MultiTenancy.Dtos
{
    public class CreateOrUpdateTenantInput : TenantEditDto
    {
        /// <summary>
        /// 管理员用户名
        /// </summary>
        public string AdminUser { get; set; }

        /// <summary>
        /// 管理员用户密码
        /// </summary>
        public string AdminUserPassword { get; set; }

        /// <summary>
        /// 管理员用户邮箱
        /// </summary>
        public string AdminUserEmail { get; set; }

        /// <summary>
        /// 管理员手机号
        /// </summary>
        public string AdminUserPhoneNumber { get; set; }
    }
}
