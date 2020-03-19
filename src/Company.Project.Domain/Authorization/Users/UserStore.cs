using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Riven.Repositories;
using Riven;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Authorization.Users
{
    public class UserStore : IUserStore<User>
    {
        protected readonly IRepository<User> _userRepo;

        public UserStore(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }


        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            //return _userRepo.InsertAsync(user);
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Check.NotNullOrWhiteSpace(userId, nameof(userId));

            if (!long.TryParse(userId, out long userIdWithLong))
            {
                throw new ArgumentException("");
            }

            return await this._userRepo.GetAll()
                .Where(o => o.Id == userIdWithLong)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
