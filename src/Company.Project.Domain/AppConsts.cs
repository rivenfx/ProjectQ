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
    }
}
