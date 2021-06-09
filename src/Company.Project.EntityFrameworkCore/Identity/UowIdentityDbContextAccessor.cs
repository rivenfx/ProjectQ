using Microsoft.EntityFrameworkCore;

using Riven.Identity;
using Riven.Uow.Providers;

using System;
using System.Collections.Generic;
using System.Text;
using Riven.Extensions;

namespace Company.Project.Identity
{
    public class UowIdentityDbContextAccessor : IIdentityDbContextAccessor
    {
        public virtual DbContext Context => _currentUnitOfWorkProvider.Current?.GetDbContext();

        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public UowIdentityDbContextAccessor(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }
    }
}
