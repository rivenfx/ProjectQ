using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Riven;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string NormalizeEmail(string email)
        {
            return email?.ToLower();
        }

        public override string NormalizeName(string name)
        {
            return name?.ToLower();
        }
    }
}
