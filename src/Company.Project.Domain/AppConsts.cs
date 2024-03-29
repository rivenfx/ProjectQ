using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{
    /// <summary>
    /// 应用常量
    /// </summary>
    public static class AppConsts
    {
        public static class Settings
        {
            /// <summary>
            /// 默认语言
            /// </summary>
            public const string DefaultLanguage = "zh-Hans";
        }


        /// <summary>
        /// 数据库相关
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// EFCore 迁移记录表名
            /// </summary>
            public const string MigrationsHistoryTableName = "EFCoreMigrationsHistory";
        }

        /// <summary>
        /// 多租户
        /// </summary>
        public static class MultiTenancy
        {
            /// <summary>
            /// 默认租户名称
            /// </summary>
            public const string DefaultTenantName = "Default";
        }

        /// <summary>
        /// 认证
        /// </summary>
        public static class Authorization
        {
            /// <summary>
            /// 系统管理员用户账号
            /// </summary>
            public const string SystemUserName = "admin";
            /// <summary>
            /// 系统管理员用户密码
            /// </summary>
            public const string SystemUserPassword = "123qwe";
            /// <summary>
            /// 系统管理员角色
            /// </summary>
            public const string SystemRoleName = "admin";


            /// <summary>
            /// 管理员用户名称
            /// </summary>
            public const string AdminUserName = nameof(AdminUserName);

            /// <summary>
            /// 管理员用户密码
            /// </summary>
            public const string AdminUserPassword = nameof(AdminUserPassword);

            /// <summary>
            /// 管理员用户邮箱
            /// </summary>
            public const string AdminUserEmail = nameof(AdminUserEmail);

            /// <summary>
            /// 管理员手机号
            /// </summary>
            public const string AdminUserPhoneNumber = nameof(AdminUserPhoneNumber);
        }
    }
}
