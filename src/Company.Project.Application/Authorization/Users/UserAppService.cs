using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Riven.Application;
using Company.Project.Authorization.Users.Dtos;
using Riven.Exceptions;
using Riven.Extensions;
using Riven.Authorization;
using Company.Project.Dtos;
using Mapster;

namespace Company.Project.Authorization.Users
{
    [PermissionAuthorize]
    public class UserAppService : IApplicationService
    {
        readonly UserManager _userManager;

        public UserAppService(UserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.User.Query)]
        public virtual async Task<PageResultDto<UserDto>> GetPage(QueryInput input)
        {
            var query = _userManager.QueryAsNoTracking
               .Where(input.QueryConditions);

            var entityTotal = await query.LongCountAsync();

            var entityList = await query
                .OrderBy(input.SortConditions)
                .Skip(input.SkipCount)
                .Take(input.PageSize)
                .ProjectToType<UserDto>()
                .ToListAsync();

            return new PageResultDto<UserDto>(entityList, entityTotal);
        }


        /// <summary>
        /// 根据用户id获取编辑dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.User.Query)]
        public virtual async Task<UserEditDto> GetEditById(Guid input)
        {
            var entity = await _userManager.QueryAsNoTracking
                .FirstOrDefaultAsync(o => o.Id == input);

            var roles = await _userManager.GetRolesAsync(entity?.Id.ToString());

            return new UserEditDto()
            {
                EntityDto = entity.Adapt<UserDto>(),
                Roles = roles.ToList()
            };

        }

        /// <summary>
        /// 创建 用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.User.Create)]
        public virtual async Task Create(CreateOrEditUserInput input)
        {
            var user = await this._userManager.CreateAsync(
                       input.EntityDto.UserName,
                       input.Password,
                       input.EntityDto.Nickname,
                       input.EntityDto.PhoneNumber,
                       input.EntityDto.PhoneNumberConfirmed,
                       input.EntityDto.Email,
                       input.EntityDto.EmailConfirmed,
                       input.EntityDto.LockoutEnabled,
                       input.EntityDto.IsActive,
                       input.EntityDto.TwoFactorEnabled
                   );
            if (input.Roles != null && input.Roles.Count > 0)
            {

                var result = await this._userManager.AddToRolesAsync(user, input.Roles);
                result.CheckError();
            }
        }

        /// <summary>
        /// 修改 用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.User.Edit)]
        public virtual async Task Update(CreateOrEditUserInput input)
        {
            if (!input.EntityDto.Id.HasValue)
            {
                return;
            }

            var user = await this._userManager.UpdateAsync(
                      input.EntityDto.Id,
                      input.Password,
                      input.EntityDto.Nickname,
                      input.EntityDto.PhoneNumber,
                      input.EntityDto.PhoneNumberConfirmed,
                      input.EntityDto.Email,
                      input.EntityDto.EmailConfirmed,
                      input.EntityDto.LockoutEnabled,
                      input.EntityDto.IsActive,
                      input.EntityDto.TwoFactorEnabled
                  );

            // 用户拥有的角色
            var currentUserRoles = await this._userManager.GetRolesAsync(user.Id.ToString());


            // 当前用户角色数量为0
            if (currentUserRoles.Count == 0)
            {
                // 输入角色存在数据,给用户添加角色
                if (!input.Roles.IsNullOrEmpty())
                {
                    (await this._userManager.AddToRolesAsync(user, input.Roles)).CheckError();
                }
                return;
            }



            // 当前用户角色数量大于0

            // 输入角色数据为空,删除用户当前拥有的角色
            if (input.Roles.IsNullOrEmpty())
            {
                (await this._userManager.RemoveFromRolesAsync(user, currentUserRoles))
                    .CheckError();
                return;
            }

            // 新增的用户角色
            var addRoles = input.Roles.Except(currentUserRoles);
            if (addRoles.Count() > 0)
            {
                (await this._userManager.AddToRolesAsync(user, addRoles))
                   .CheckError();
            }

            // 删除的用户角色
            var removeRoles = currentUserRoles.Except(input.Roles);
            if (removeRoles.Count() > 0)
            {
                (await this._userManager.RemoveFromRolesAsync(user, removeRoles))
                   .CheckError();
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="idList">用户id集合</param>
        /// <returns></returns>
        [PermissionAuthorize(AppPermissions.User.Delete)]
        public virtual async Task Delete(List<Guid> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return;
            }

            await this._userManager.DeleteAsync(o => idList.Contains(o.Id));
        }

    }
}
