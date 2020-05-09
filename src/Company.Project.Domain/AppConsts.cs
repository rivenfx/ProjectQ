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
            /// 系统管理员显示名称
            /// </summary>
            public const string SystemRoleDisplayName = "Administrator";
        }
    }
}
