using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Riven;
using Riven.Repositories;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore : IRoleStore<Role>,
        IQueryableRoleStore<Role>,
        IRoleClaimStore<Role>
    {
        private bool _disposed;


        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IRepository<Role> _roleRepo;
        protected readonly IRepository<RoleClaim, int> _roleClaimRepo;

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IRepository<Role> roleRepo, IRepository<RoleClaim, int> roleClaimRepo)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _roleRepo = roleRepo;
            _roleClaimRepo = roleClaimRepo;

            ErrorDescriber = new IdentityErrorDescriber();

        }

        protected IdentityErrorDescriber ErrorDescriber { get; }

        public IQueryable<Role> Roles => this._roleRepo.GetAll().AsNoTracking();

        public IQueryable<RoleClaim> RoleClaims => this._roleClaimRepo.GetAll().AsNoTracking();

        public bool AutoSaveChanges { get; set; } = true;

        #region IRoleStore 实现

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            await _roleRepo.InsertAsync(role);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            await this._roleRepo.DeleteAsync(role);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var id = ConvertIdFromString(roleId);
            return await this._roleRepo.GetAll()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(r => r.Id.Equals(roleId), cancellationToken);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await this._roleRepo.GetAll()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(ConvertIdToString(role.Id));
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            await this._roleRepo.UpdateAsync(role);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }


        #endregion

        #region IRoleClaimStore 实现

        public async Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
        }

        public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            var roleClaim = CreateRoleClaim(role, claim);
            this._roleClaimRepo.InsertAsync(roleClaim);

            return Task.FromResult(false);
        }

        public async Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            var claims = await RoleClaims
                .Where(rc => rc.RoleId.Equals(role.Id) 
                    && rc.ClaimValue == claim.Value 
                    && rc.ClaimType == claim.Type)
                .ToListAsync(cancellationToken);
            foreach (var c in claims)
            {
                await this._roleClaimRepo.DeleteAsync(c);
            }
        }


        #endregion



        #region 释放资源

        public void Dispose()
        {
            _disposed = true;
        }

        #endregion

        #region 内部辅助函数

        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual Task SaveChanges(CancellationToken cancellationToken)
        {
            if (!AutoSaveChanges || _unitOfWorkManager.Current == null)
            {
                return Task.CompletedTask;
            }


            return this._unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        protected virtual string ConvertIdToString(long id)
        {
            if (object.Equals(id, default(long)))
            {
                return null;
            }
            return id.ToString();
        }

        protected virtual long ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(long);
            }
            return (long)TypeDescriptor.GetConverter(typeof(long)).ConvertFromInvariantString(id);
        }

        protected virtual RoleClaim CreateRoleClaim(Role role, Claim claim)
           => new RoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };

        #endregion
    }
}
