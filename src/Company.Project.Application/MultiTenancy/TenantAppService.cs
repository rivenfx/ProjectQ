using Company.Project.MultiTenancy.Dtos;

using Riven.Application;
using Riven.Authorization;
using Riven.Uow;
using Riven.Extensions;

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Riven.Data;
using Company.Project.Dtos;
using Riven.Linq;
using Riven.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Company.Project.MultiTenancy
{
    public class TenantAppService : AppServiceBase, IApplicationService
    {
        readonly ITenantManager _tenantManager;

        public TenantAppService(
            IServiceProvider serviceProvider,
            ITenantManager tenantManager
            )
            : base(serviceProvider)
        {
            _tenantManager = tenantManager;
        }

        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            using (var uow = this.UnitOfWorkManager.Begin())
            {

                var tenant = await _tenantManager.GetByName(input.TenantName);

                var result = new IsTenantAvailableOutput();
                result.TenantName = tenant?.Name;

                if (tenant == null)
                {
                    result.State = TenantAvailabilityState.NotFound;
                }
                else
                {
                    result.State = tenant.IsActive ? TenantAvailabilityState.Available : TenantAvailabilityState.InActive;
                }

                return result;

            }

        }

        [PermissionAuthorize(AppPermissions.Tenant.Node, Scope = PermissionAuthorizeScope.Host)]
        public virtual async Task<PageResultDto<TenantDto>> GetPage(QueryInput input)
        {
            var query = this._tenantManager.QueryAsNoTracking
                .Where(input.QueryConditions)
                ;

            var total = await query.LongCountAsync();

            var dtoList = await query.OrderBy(input.SortConditions)
                .Skip(input.SkipCount)
                .Take(input.PageSize)
                .ProjectToType<TenantDto>()
                .ToListAsync();


            return new PageResultDto<TenantDto>(dtoList, total);

        }


        [PermissionAuthorize(AppPermissions.Tenant.Query, Scope = PermissionAuthorizeScope.Host)]
        public virtual async Task<TenantEditDto> GetEditById(Guid input)
        {
            var tenant = await this._tenantManager.FindById(input);

            var tenantDto = tenant.MapTo<TenantDto>();
            tenantDto.ConnectionString = null;


            return new TenantEditDto()
            {
                EntityDto = tenantDto
            };
        }



        [PermissionAuthorize(AppPermissions.Tenant.Create, Scope = PermissionAuthorizeScope.Host)]
        public virtual async Task Create(CreateTenantInput input)
        {
            // 创建租户
            var tenant = await this._tenantManager.Create(
                input.EntityDto.Name,
                input.EntityDto.DisplayName,
                input.EntityDto.Description,
                input.EntityDto.ConnectionString,
                input.EntityDto.IsStatic,
                input.EntityDto.IsActive
                );

            // 创建种子数据 默认的用户和权限
            using (this.CurrentUnitOfWork.ChangeTenant(tenant.Name))
            {
                var seeder = this.GetService<IDataSeeder>();
                var seedContext = new DataSeedContext(tenant.Name)
                    .WithProperty(AppConsts.Authorization.AdminUserName, input.AdminUser)
                    .WithProperty(AppConsts.Authorization.AdminUserPassword, input.AdminUserPassword)
                    .WithProperty(AppConsts.Authorization.AdminUserEmail, input.AdminUserEmail)
                    .WithProperty(AppConsts.Authorization.AdminUserPhoneNumber, input.AdminUserPhoneNumber)
                    ;

                await seeder.Run(seedContext);
            }
        }

        public virtual async Task Update(TenantEditDto input)
        {
            await this._tenantManager.Update(
                input.EntityDto.Name,
                input.EntityDto.DisplayName,
                input.EntityDto.Description
                );
        }
    }
}
