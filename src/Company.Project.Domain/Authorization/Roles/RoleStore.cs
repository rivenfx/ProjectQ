using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Roles;
using Riven.Uow;

using Company.Project.Authorization.Users;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Linq;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore<TDbContext> : AppRoleStore<Role, TDbContext, Guid, UserRole, RolePermission>
        where TDbContext : DbContext
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        protected virtual DbSet<RolePermission> RolePermissions { get { return Context.Set<RolePermission>(); } }

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override TDbContext Context => this._unitOfWorkManager.Current.GetDbContext() as TDbContext;


        public override async Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await RolePermissions.Where(rc => rc.RoleId.Equals(role.Id))
                .Select(c => new Claim(c.Name, c.Name))
                .ToListAsync(cancellationToken);
        }


        public override async Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            var claims = await RolePermissions.Where(rc => rc.RoleId.Equals(role.Id) && rc.Name == claim.Value)
                .ToListAsync(cancellationToken);
            foreach (var c in claims)
            {
                RolePermissions.Remove(c);
            }
        }
    }
}
