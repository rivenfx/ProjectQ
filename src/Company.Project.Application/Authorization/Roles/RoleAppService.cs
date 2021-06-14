using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Riven.Application;
using Riven.Exceptions;
using Riven.Extensions;
using Company.Project.Authorization.Roles.Dtos;
using System.Security.Claims;
using Riven.Authorization;
using Company.Project.Authorization.Users.Dtos;
using Company.Project.Dtos;
using Riven;
using JetBrains.Annotations;

namespace Company.Project.Authorization.Roles
{

    [PermissionAuthorize]
    public class RoleAppService : IApplicationService
    {
        readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }


        /// <summary>
        /// 分页查询所有角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Query)]
        public virtual async Task<PageResultDto<RoleDto>> GetPage(QueryInput input)
        {
            var query = _roleManager.QueryAsNoTracking
                .Where(input.QueryConditions);

            var entityTotal = await query.LongCountAsync();

            var entityList = await query
                .OrderBy(input.SortConditions)
                .Skip(input.SkipCount)
                .Take(input.PageSize)
                .ProjectTo<RoleDto>()
                .ToListAsync();

            return new PageResultDto<RoleDto>(entityList, entityTotal);
        }


        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Query)]
        public virtual async Task<ListResultDto<RoleDto>> GetAll()
        {
            var entityList = await _roleManager.QueryAsNoTracking
                .ProjectTo<RoleDto>()
                .ToListAsync();

            return new ListResultDto<RoleDto>(entityList);
        }


        /// <summary>
        /// 根据角色id获取编辑dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Query)]
        public virtual async Task<RoleEditDto> GetEditById(Guid input)
        {
            var entity = await _roleManager.QueryAsNoTracking
                .FirstOrDefaultAsync(o => o.Id == input);

            var permissions = await _roleManager.GetPermissionsAsync(entity.Name);

            return new RoleEditDto()
            {
                EntityDto = entity.MapTo<RoleDto>(),
                Permissions = permissions.ToList()
            };

        }

        /// <summary>
        /// 创建 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Create)]
        public virtual async Task Create(CreateOrUpdateRoleInput input)
        {
            await _roleManager.CreateAsync(
                input.EntityDto.Name,
                input.EntityDto.DisplayName,
                input.EntityDto.Description,
                permissions: input.Permissions?.ToArray()
                );
        }

        /// <summary>
        /// 修改 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Edit)]
        public virtual async Task Update(CreateOrUpdateRoleInput input)
        {
            if (!input.EntityDto.Id.HasValue)
            {
                return;
            }

            await _roleManager.UpdateAsync(input.EntityDto.Id, input.EntityDto.DisplayName, input.EntityDto.Description, input.Permissions?.ToArray());
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="idList">角色id集合</param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.Role.Delete)]
        public virtual async Task Delete(List<Guid> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return;
            }

            await this._roleManager.DeleteAsync(o => idList.Contains(o.Id));
        }
    }
}
