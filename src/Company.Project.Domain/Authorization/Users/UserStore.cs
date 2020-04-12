using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Users;
using Riven.Uow;

using Company.Project.Authorization.Roles;

namespace Company.Project.Authorization.Users
{
    public class UserStore<TDbContext> : AppUserStore<User, Role, TDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
         where TDbContext : DbContext
    {
        protected readonly IAppIdentityStoreSessionAccessor _identityStoreSessionAccessor;

        public UserStore(IAppIdentityStoreSessionAccessor identityStoreSessionAccessor, IdentityErrorDescriber describer = null)
            : base()
        {
            this._identityStoreSessionAccessor = identityStoreSessionAccessor;
        }

        public override TDbContext Context => this._identityStoreSessionAccessor.GetSession<TDbContext>();
    }
}
