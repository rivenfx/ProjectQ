using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Roles;
using Riven.Uow;

using Company.Project.Authorization.Users;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore<TDbContext> : AppRoleStore<Role, TDbContext, long, UserRole, RoleClaim>
        where TDbContext : DbContext
    {
        protected readonly IAppIdentityStoreSessionAccessor _identityStoreSessionAccessor;

        public RoleStore(IAppIdentityStoreSessionAccessor identityStoreSessionAccessor, IdentityErrorDescriber describer = null)
            : base()
        {
            this._identityStoreSessionAccessor = identityStoreSessionAccessor;
        }

        public override TDbContext Context => this._identityStoreSessionAccessor.GetSession<TDbContext>();
    }
}
