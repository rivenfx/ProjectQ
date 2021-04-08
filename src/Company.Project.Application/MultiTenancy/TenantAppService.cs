using Company.Project.MultiTenancy.Dtos;

using Riven.Application;
using Riven.Authorization;
using Riven.Uow;
using Riven.Extensions;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riven.Data;
using Company.Project.Dtos;

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

            return null;

        }


        [PermissionAuthorize(AppPermissions.Tenant.Query, Scope = PermissionAuthorizeScope.Host)]
        public virtual async Task<object> GetEditById(Guid input)
        {
            var tenant = await this._tenantManager.FindById(input);


            new TenantDto()
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DisplayName = tenant.DisplayName,
                Description = tenant.Description,
                IsActive = tenant.IsActive,
                IsStatic = tenant.IsStatic
            };


            return null;
        }


        [PermissionAuthorize(AppPermissions.Tenant.Create, AppPermissions.Tenant.Edit, Scope = PermissionAuthorizeScope.Host)]
        public virtual async Task CreateOrUpdate(CreateOrUpdateTenantInput input)
        {
            if (!input.EntityDto.Id.HasValue)
            {
                await this.Create(input);
            }
            else
            {
                await this.Update(input);
            }
        }


        [PermissionAuthorize(AppPermissions.Tenant.Create, Scope = PermissionAuthorizeScope.Host)]
        protected virtual async Task Create(CreateOrUpdateTenantInput input)
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

        [PermissionAuthorize(AppPermissions.Tenant.Edit, Scope = PermissionAuthorizeScope.Host)]
        protected virtual async Task Update(CreateOrUpdateTenantInput input)
        {
            await this._tenantManager.Update(
                input.EntityDto.Name,
                input.EntityDto.DisplayName,
                input.EntityDto.Description
                );
        }
    }
}
