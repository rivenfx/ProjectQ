﻿using Microsoft.AspNetCore.Identity;
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
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override TDbContext Context => _unitOfWorkManager.Current.GetDbContext() as TDbContext;
    }
}