using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riven.Extensions;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users
{
    public class AppUserStore<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>
         where TUser : IdentityUser<TKey>
         where TRole : IdentityRole<TKey>
         where TContext : DbContext
         where TKey : IEquatable<TKey>
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppUserStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }

    public class AppUserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppUserStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }
}
