using Riven.Data;

using System;
using System.Threading.Tasks;
using Riven.Extensions;
using Riven.Repositories;
using Company.Project.MultiTenancy;
using Riven;

namespace Company.Project.SeedData
{
    /// <summary>
    /// 租户创建器
    /// </summary>
    public class TenantDataSeeder : IDataSeedExecutor
    {
        protected readonly IGuidGenerator _guidGenerator;
        protected readonly IRepository<Tenant, Guid> _tenantRepo;

        public TenantDataSeeder(IGuidGenerator guidGenerator, IRepository<Tenant, Guid> tenantRepo)
        {
            _guidGenerator = guidGenerator;
            _tenantRepo = tenantRepo;
        }

        public async Task Run(DataSeedContext dataSeedContext)
        {
            // 只有host时才执行创建租户
            if (!dataSeedContext.TenantName.IsNullOrWhiteSpace())
            {
                return;
            }

            // 获取默认的租户
            var tenant = await _tenantRepo
                .FirstOrDefaultAsync(o => o.Name == AppConsts.MultiTenancy.DefaultTenantName);
            if (tenant == null)
            {
                tenant = new Tenant()
                {
                    Id = this._guidGenerator.Create(),
                    Name = AppConsts.MultiTenancy.DefaultTenantName,
                    DisplayName = AppConsts.MultiTenancy.DefaultTenantName,
                    Description = AppConsts.MultiTenancy.DefaultTenantName,
                    IsStatic = true,
                    IsActive = true
                };

                await _tenantRepo.InsertAsync(tenant);
            }

        }
    }
}
