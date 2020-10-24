using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Riven.Extensions;
using Riven.Identity.Users;
using Riven.Uow;

using Company.Project.Authorization.Roles;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Company.Project.Authorization.Users
{
    public class UserStore<TDbContext> : AppUserStore<User, Role, TDbContext, Guid, UserPermission, UserRole, UserLogin, UserToken, RolePermission>
         where TDbContext : DbContext
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        protected virtual DbSet<UserPermission> UserPermissions { get { return Context.Set<UserPermission>(); } }

        public UserStore(IUnitOfWorkManager unitOfWorkManager, IdentityErrorDescriber describer = null)
            : base()
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override TDbContext Context => this._unitOfWorkManager.Current.GetDbContext() as TDbContext;


        public override async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            var matchedPermissions = await UserPermissions
                .Where(uc => uc.UserId.Equals(user.Id) && uc.Name == claim.Value)
                .ToListAsync(cancellationToken);
            foreach (var matchedPermission in matchedPermissions)
            {
                matchedPermission.Name = newClaim.Value;
            }
        }

        public override async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            foreach (var claim in claims)
            {
                var matchedPermissions = await UserPermissions
                    .Where(uc => uc.UserId.Equals(user.Id) && uc.Name == claim.Value)
                    .ToListAsync(cancellationToken);

                foreach (var matchedPermission in matchedPermissions)
                {
                    UserPermissions.Remove(matchedPermission);
                }
            }
        }

        public override async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var query = from userPermission in UserPermissions
                        join user in Users on userPermission.UserId equals user.Id
                        where userPermission.ClaimValue == claim.Value
                        select user;

            return await query.ToListAsync(cancellationToken);
        }
    }
}
