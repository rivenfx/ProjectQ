using Company.Project.Authorization.Roles;
using Company.Project.Database;
using Microsoft.AspNetCore.Identity;
using Riven.Extensions;
using Riven.Identity.Users;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class UserStore : AppUserStore<User, Role, AppDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserStore(IUnitOfWorkManager unitOfWorkManager,IdentityErrorDescriber describer=null)
            :base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override AppDbContext Context => _unitOfWorkManager.Current.GetDbContext() as AppDbContext;
    }
}
