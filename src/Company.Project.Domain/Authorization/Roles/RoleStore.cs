using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Roles;
using Riven.Uow;

using Company.Project.Authorization.Users;
using System;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore<TDbContext> : AppRoleStore<Role, TDbContext, Guid, UserRole, RolePermission>
        where TDbContext : DbContext
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override TDbContext Context => this._unitOfWorkManager.Current.GetDbContext() as TDbContext;
    }
}
