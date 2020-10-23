using Company.Project.Authorization.Extenstions;
using Company.Project.Authorization.Roles;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Riven;
using Riven.Authorization;
using Riven.Exceptions;
using Riven.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Users
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    public class UserManager : UserManager<User>, IUserRolePermissionAccessor
    {
        public IQueryable<User> Query => this.Users;
        public IQueryable<User> QueryAsNoTracking => this.Users.AsNoTracking();

        public UserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger
            ) : base(store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {

        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="password">密码</param>
        /// <param name="nickname">昵称</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="phoneNumberConfirmed">手机号已确认</param>
        /// <param name="email">邮箱</param>
        /// <param name="emailConfirmed">邮箱已确认</param>
        /// <param name="lockoutEnabled">是否启用锁定</param>
        /// <param name="isActive">是否激活</param>
        /// <param name="isStatic">是否为内置</param>
        /// <param name="twoFactorEnabled">是否启用双重验证,默认为false</param>
        /// <returns></returns>
        public virtual async Task<User> CreateAsync([NotNull] string userName, [NotNull] string password, [NotNull] string nickname, string phoneNumber, bool phoneNumberConfirmed, string email, bool emailConfirmed, bool lockoutEnabled, bool isActive, bool isStatic = false, bool twoFactorEnabled = false)
        {
            Check.NotNullOrWhiteSpace(userName, nameof(userName));
            Check.NotNullOrWhiteSpace(password, nameof(password));

            var user = new User()
            {
                UserName = userName,
                Nickname = nickname,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                Email = email,
                EmailConfirmed = emailConfirmed,
                LockoutEnabled = lockoutEnabled,
                IsActive = isActive,
                TwoFactorEnabled = twoFactorEnabled,
                Code = Guid.NewGuid().ToString(),
                IsStatic = isStatic
            };

            var result = await this.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("创建用户时发生错误", detiles.ToString());
            }

            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="nickname">昵称</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="phoneNumberConfirmed">手机号已确认</param>
        /// <param name="email">邮箱</param>
        /// <param name="emailConfirmed">邮箱已确认</param>
        /// <param name="lockoutEnabled">是否启用锁定</param>
        /// <param name="isActive">是否激活</param>
        /// <param name="isStatic">是否为内置</param>
        /// <param name="twoFactorEnabled">是否启用双重验证,默认为false</param>
        /// <returns></returns>
        public virtual async Task<User> UpdateAsync(Guid? id, string password, [NotNull] string nickname, string phoneNumber, bool phoneNumberConfirmed, string email, bool emailConfirmed, bool lockoutEnabled, bool isActive, bool isStatic = false, bool twoFactorEnabled = false)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(nickname, nameof(nickname));

            var user = await this.FindByIdAsync(id.Value.ToString());
            if (user == null)
            {
                throw new UserFriendlyException($"未找到用户: {nickname}");
            }



            user.Nickname = nickname;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = phoneNumberConfirmed;
            user.Email = email;
            user.EmailConfirmed = emailConfirmed;
            user.LockoutEnabled = lockoutEnabled;
            user.IsActive = isActive;
            user.TwoFactorEnabled = twoFactorEnabled;


            if (!password.IsNullOrWhiteSpace())
            {
                user.PasswordHash = this.PasswordHasher.HashPassword(user, password);
            }

            var result = await this.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("修改用户时发生错误", detiles.ToString());
            }

            return user;
        }

        /// <summary>
        /// 添加 claims
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public virtual async Task AddPermissionsAsync([NotNull] User user, params string[] permissions)
        {
            Check.NotNull(user, nameof(user));

            if (permissions == null || permissions.Length == 0)
            {
                return;
            }

            var permissionsDistinct = permissions.Distinct().Where(o => !o.IsNullOrWhiteSpace());
            if (permissionsDistinct.Count() == 0)
            {
                return;
            }

            var identityResult = await this.AddClaimsAsync(user, permissionsDistinct.ToClaims());

            if (!identityResult.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in identityResult.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("用户添加权限失败!", detiles.ToString());
            }

        }

        /// <summary>
        /// 添加 Roles, 不要直接使用 AddToRoleAsync
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roles">角色名称</param>
        /// <returns></returns>
        public virtual async Task AddToRolesAsync([NotNull] User user, params string[] roles)
        {
            Check.NotNull(user, nameof(user));
            if (roles == null || roles.Length == 0)
            {
                return;
            }

            var rolesDistinct = roles.Distinct().Where(o => !o.IsNullOrWhiteSpace());
            if (rolesDistinct.Count() == 0)
            {
                return;
            }

            var identityResult = await this.AddToRolesAsync(user, rolesDistinct);
            if (!identityResult.Succeeded)
            {
                var detiles = new StringBuilder();
                foreach (var error in identityResult.Errors)
                {
                    detiles.AppendLine($"{error.Code}: {error.Description}");
                }
                throw new UserFriendlyException("用户添加角色失败!", detiles.ToString());
            }
        }


        /// <summary>
        /// 根据用户名/邮箱/手机号码查找用户
        /// </summary>
        /// <param name="userNameOrEmailOrPhoneNumber"></param>
        /// <returns></returns>
        public virtual async Task<User> FindByNameOrEmailOrPhoneNumberAsync(string userNameOrEmailOrPhoneNumber)
        {
            Check.NotNullOrEmpty(userNameOrEmailOrPhoneNumber, nameof(userNameOrEmailOrPhoneNumber));

            var userName = this.NormalizeName(userNameOrEmailOrPhoneNumber);
            var email = this.NormalizeEmail(userNameOrEmailOrPhoneNumber);

            var user = await this.Query
                .FirstOrDefaultAsync(o => o.NormalizedUserName == userName
                                    || o.NormalizedEmail == email
                                    || o.PhoneNumber == userNameOrEmailOrPhoneNumber);

            return user;
        }

        /// <summary>
        /// 根据条件删除用户
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<User>> DeleteAsync([NotNull] Expression<Func<User, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));

            var users = this.Users.AsNoTracking().Where(o => !o.IsStatic).Where(predicate);
            if ((await users.CountAsync()) == 0)
            {
                return users;
            }

            foreach (var user in users)
            {
                if (user.IsStatic)
                {
                    throw new UserFriendlyException("不能删除系统用户!");
                }

                var identityResult = await this.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    var detiles = new StringBuilder();
                    foreach (var error in identityResult.Errors)
                    {
                        detiles.AppendLine($"{error.Code}: {error.Description}");
                    }
                    throw new UserFriendlyException("删除用户失败!", detiles.ToString());
                }
            }

            return users;
        }

        public override string NormalizeEmail(string email)
        {
            return email?.ToLower();
        }

        public override string NormalizeName(string name)
        {
            return name?.ToLower();
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return FindByNameOrEmailOrPhoneNumberAsync(userName);
            //return base.FindByNameAsync(userName);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return FindByNameOrEmailOrPhoneNumberAsync(email);
        }

        public async Task<IList<Claim>> GetPermissionsByUserIdAsync([NotNull] string userId)
        {
            Check.NotNullOrWhiteSpace(userId, nameof(userId));

            var user = await this.FindByIdAsync(userId);

            return await this.GetClaimsAsync(user);
        }

        public async Task<IList<Claim>> GetPermissionsByUserNameAsync([NotNull] string userName)
        {
            Check.NotNullOrWhiteSpace(userName, nameof(userName));

            var user = await this.FindByNameAsync(userName);

            return await this.GetClaimsAsync(user);
        }

        public async Task<IList<string>> GetRolesByUserIdAsync([NotNull] string userId)
        {
            Check.NotNullOrWhiteSpace(userId, nameof(userId));

            var user = await this.FindByIdAsync(userId);

            return await this.GetRolesAsync(user);
        }

        public async Task<IList<string>> GetRolesByUserNameAsync([NotNull] string userName)
        {
            Check.NotNullOrWhiteSpace(userName, nameof(userName));

            var user = await this.FindByNameAsync(userName);

            return await this.GetRolesAsync(user);
        }
    }
}
