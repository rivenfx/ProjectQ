using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Users;
using Riven.Uow;

using Company.Project.Authorization.Roles;
using System;

namespace Company.Project.Authorization.Users
{
    public class UserStore<TDbContext> : AppUserStore<User, Role, TDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
         where TDbContext : DbContext
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override TDbContext Context => this._unitOfWorkManager.Current.GetDbContext() as TDbContext;
    }
}
