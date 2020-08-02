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
using Riven.Identity.Authorization;
using Company.Project.Authorization.Users.Dtos;
using Company.Project.Dtos;
using Mapster;
using JetBrains.Annotations;
using Riven.Exceptions;

namespace Company.Project.Authorization.Roles
{

    [ClaimsAuthorize]
    public class RoleAppService : IApplicationService
    {
        readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }


        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ClaimsAuthorize(AppClaimsConsts.User.Query)]
        public virtual async Task<PageResultDto<RoleDto>> GetAll(QueryInput input)
        {
            var query = _roleManager.QueryAsNoTracking
                .Skip(input.SkipCount)
                .Take(input.PageSize);

            var entityTotal = await query.LongCountAsync();

            var entityList = await query
                .ProjectToType<RoleDto>()
                .ToListAsync();

            return new PageResultDto<RoleDto>(entityList, entityTotal);
        }


        /// <summary>
        /// 根据角色id获取编辑dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<RoleEditDto> GetEditById(Guid input)
        {
            var entity = await _roleManager.QueryAsNoTracking
                .FirstOrDefaultAsync(o => o.Id == input);

            var claims = await _roleManager.GetClaimsByRoleIdAsync(entity?.Id.ToString());

            return new RoleEditDto()
            {
                EntityDto = entity.Adapt<RoleDto>(),
                Claims = claims.Select(o => o.Type).ToList()
            };

        }

        /// <summary>
        /// 创建 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ClaimsAuthorize(AppClaimsConsts.Role.Create)]
        public virtual async Task Create(CreateOrUpdateRoleInput input)
        {
            await _roleManager.CreateAsync(
                input.EntityDto.Name,
                input.EntityDto.DisplayName,
                input.EntityDto.Description,
                claims: input.Claims?.ToArray()
                );
        }

        /// <summary>
        /// 修改 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ClaimsAuthorize(AppClaimsConsts.Role.Edit)]
        public virtual async Task Update(CreateOrUpdateRoleInput input)
        {
            if (!input.EntityDto.Id.HasValue)
            {
                return;
            }

            await _roleManager.UpdateAsync(input.EntityDto.Id, input.EntityDto.DisplayName, input.EntityDto.Description, input.Claims?.ToArray());
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="idList">角色id集合</param>
        /// <returns></returns>
        [ClaimsAuthorize(AppClaimsConsts.Role.Delete)]
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
