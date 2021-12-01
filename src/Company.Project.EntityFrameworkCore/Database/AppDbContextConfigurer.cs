using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Company.Project.Configuration;
using Microsoft.Extensions.Logging;
using Riven.Common;

namespace Company.Project.Database
{
    public static class AppDbContextConfigurer
    {
        public static readonly ILoggerFactory DbLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        /// <summary>
        /// 配置DbContext
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="connection">连接字符串</param>
        public static void Configure(
            this DbContextOptionsBuilder builder,
            IConfiguration configuration,
            string connectionString)
        {
            switch (configuration.GetDatabaseType())
            {
                case DatabaseType.SqlServer:
                    builder.UseSqlServer(connectionString, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
                case DatabaseType.PostgreSQL:
                    builder.UseRivenPostgreSQL(connectionString, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName.ToLower());
                    });
                    break;
                case DatabaseType.MySql:
                    builder.UseMySql(connectionString, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
                case DatabaseType.Oracle:
                    builder.UseOracle(connectionString, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
            }

            builder.AddDbLogger();
        }

        /// <summary>
        /// 配置DbContext
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="connection">现有连接</param>
        public static void Configure(
            this DbContextOptionsBuilder builder,
            IConfiguration configuration,
            DbConnection connection)
        {
            switch (configuration.GetDatabaseType())
            {
                case DatabaseType.SqlServer:
                    builder.UseSqlServer(connection, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
                case DatabaseType.PostgreSQL:
                    builder.UseRivenPostgreSQL(connection, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
                case DatabaseType.MySql:
                    builder.UseMySql(connection, (options) =>
                    {
                        options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
                    });
                    break;
            }

            builder.AddDbLogger();
        }


        /// <summary>
        /// 添加日志, Debug时生效
        /// </summary>
        /// <param name="builder"></param>
        public static void AddDbLogger(this DbContextOptionsBuilder builder)
        {
            #region DEBUG
            builder.UseLoggerFactory(DbLoggerFactory); 
            #endregion
        }
    }
}
