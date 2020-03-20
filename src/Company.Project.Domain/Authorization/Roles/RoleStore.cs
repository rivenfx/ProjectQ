using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Riven;
using Riven.Repositories;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Company.Project.Authorization.Roles
{
    public class RoleStore : IRoleStore<Role>
    {
        private bool _disposed;


        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IRepository<Role> _roleRepo;

        protected IdentityErrorDescriber ErrorDescriber { get; }

        public RoleStore(IUnitOfWorkManager unitOfWorkManager, IRepository<Role> roleRepo)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _roleRepo = roleRepo;

            ErrorDescriber = new IdentityErrorDescriber();
        }

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
        public void Dispose()
        {
            _disposed = true;
        }

        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual async Task SaveChanges(CancellationToken cancellationToken)
        {
            await this._unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
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
    }
}
