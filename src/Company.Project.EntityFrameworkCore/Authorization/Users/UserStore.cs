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
    public class UserStore : IdentityUserStore<User, Role, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserStore(IServiceProvider serviceProvider, IdentityErrorDescriber describer = null)
            : base(serviceProvider, describer)
        {
        }


    }
}
