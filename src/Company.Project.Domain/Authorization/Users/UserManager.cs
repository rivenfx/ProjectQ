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
using Riven.Identity.Users;

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
    public class UserManager : UserManager<User>
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
                IsStatic = isStatic
            };

            (await this.CreateAsync(user, password))
                .CheckError(true, "创建用户时发生错误!");

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

            (await this.UpdateAsync(user))
                .CheckError(true, "修改用户时发生错误!");

            return user;
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

            var user = await this.QueryAsNoTracking
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

            var users = await this.Users.AsNoTracking().Where(o => !o.IsStatic).Where(predicate).ToListAsync();
            if (users.Count == 0)
            {
                return users;
            }

            foreach (var user in users)
            {
                if (user.IsStatic)
                {
                    throw new UserFriendlyException("不能删除系统用户!");
                }

                (await this.DeleteAsync(user))
                    .CheckError(true, "删除用户失败!");

                var userRoles = await this.GetRolesAsync(user);
                (await this.RemoveFromRolesAsync(user, userRoles))
                    .CheckError(true, "删除用户角色失败!");
            }

            return users;
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return FindByNameOrEmailOrPhoneNumberAsync(userName);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return FindByNameOrEmailOrPhoneNumberAsync(email);
        }


        public virtual async Task<IList<string>> GetRolesAsync(string userId)
        {
            var userRoleFinder = this.Store as IIdentityUserRoleFinder;
            return (await userRoleFinder.GetRolesByUserIdAsync(userId)).ToList();
        }
    }
}
