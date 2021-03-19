using Microsoft.EntityFrameworkCore;

using Riven.Identity;
using Riven.Uow.Providers;

using System;
using System.Collections.Generic;
using System.Text;
using Riven.Extensions;

namespace Company.Project.Identity
{
    public class UowIDbContextAccessor : IIdentityDbContextAccessor
    {
        public virtual DbContext Context => _currentUnitOfWorkProvider.Current?.GetDbContext();

        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public UowIDbContextAccessor(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }
    }
}
