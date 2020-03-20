using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riven.Extensions;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class AppRoleStore<TRole, TContext, TKey> : RoleStore<TRole, TContext, TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppRoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }

    public class AppRoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        : RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        protected IUnitOfWorkManager _unitOfWorkManager;

        public AppRoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base(unitOfWorkManager.Current.GetDbContext() as TContext, describer)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override TContext Context => this._unitOfWorkManager.Current.GetDbContext() as TContext;
    }
}
