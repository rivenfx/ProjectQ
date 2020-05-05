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

namespace Company.Project.Authorization.Roles
{
    public class RoleAppService : IApplicationService
    {
        readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }


        /// <summary>
        /// 创建 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ClaimsAuthorize(AppClaimsConsts.Role.Create)]
        public virtual async Task Create(CreateOrUpdateRoleInput input)
        {
            await _roleManager.CreateAsync(input.EntityDto.Name, input.EntityDto.DisplayName, input.EntityDto.Description, input.Claims?.ToArray());
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
        public virtual async Task Delete(List<long> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return;
            }

            await this._roleManager.DeleteAsync(o => idList.Contains(o.Id));
        }
    }
}
