using Company.Project.Authorization.Users;
using Company.Project.Database;
using Microsoft.AspNetCore.Identity;
using Riven.Extensions;
using Riven.Identity.Roles;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore : AppRoleStore<Role, AppDbContext, long, UserRole, RoleClaim>
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override AppDbContext Context => _unitOfWorkManager.Current.GetDbContext() as AppDbContext;
    }
}
