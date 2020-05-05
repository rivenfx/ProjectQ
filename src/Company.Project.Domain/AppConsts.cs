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
        /// <summary>
        /// 默认语言
        /// </summary>
        public const string DefaultLanguage = "zh-Hans";

        /// <summary>
        /// 应用名称键值
        /// </summary>
        public const string AppNameKey = "App:Name";

        /// <summary>
        /// 应用版本键值
        /// </summary>
        public const string AppVersionKey = "App:Version";


        /// <summary>
        /// 数据库相关
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// 数据库连接字符串键值
            /// </summary>
            public const string ConnectionStringKey = "ConnectionStrings:Default";

            /// <summary>
            /// EFCore 迁移记录表名
            /// </summary>
            public const string MigrationsHistoryTableName = "EFCoreMigrationsHistory";
        }

        public static class Identity
        {
            public const string Issuer = "ProjectQ";
        }
    }
}
