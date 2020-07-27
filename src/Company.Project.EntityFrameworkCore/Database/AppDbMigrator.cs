using System;
using System.Linq;
using Riven.MultiTenancy;
using Riven.Dependency;
using Riven.Uow;
using Riven;
using System.Transactions;
using Riven.Uow.Providers;
using Riven.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Database
{
    public class AppDbMigrator : IDbMigrator, ITransientDependency
    {
        readonly IUnitOfWorkManager _unitOfWorkManager;

        public AppDbMigrator(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void CreateOrMigrateForHost()
        {
            CreateOrMigrate(null);
        }

        public void CreateOrMigrateForTenant(string tenantName)
        {
            if (tenantName.IsNullOrWhiteSpace())
            {
                return;
            }

            CreateOrMigrate(tenantName);
        }

        protected virtual void CreateOrMigrate(string tenantName, Action<DbContext> seedAction = null)
        {
            var unitOfWorkOptions = new UnitOfWorkOptions();

            // 工作单元级别
            unitOfWorkOptions.Scope = TransactionScopeOption.Suppress;
            // 当前连接字符串名称
            unitOfWorkOptions.ConnectionStringName = tenantName.IsNullOrWhiteSpace() ? RivenUnitOfWorkConsts.DefaultConnectionStringName : tenantName;


            using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                // 获取当前数据库上下文
                var dbContext = _unitOfWorkManager.Current.GetDbContext();
                dbContext.Database.Migrate();

                // 种子数据
                seedAction?.Invoke(dbContext);

                // 保存
                _unitOfWorkManager.Current.SaveChanges();

                // 提交工作单元
                uow.Complete();
            }
        }
    }
}