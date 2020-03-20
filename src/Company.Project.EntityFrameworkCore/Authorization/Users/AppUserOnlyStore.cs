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
    public class AppUserOnlyStore<TUser, TContext, TKey> : UserOnlyStore<TUser, TContext, TKey>
        where TUser : IdentityUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppUserOnlyStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }

    public class AppUserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
        : UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : IdentityUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppUserOnlyStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }
}
