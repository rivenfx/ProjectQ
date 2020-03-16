using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Company.Project.Samples;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riven;
using Riven.Identity.Roles;
using Riven.Identity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContext
        : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AppDbContext()
            : base()
        {

        }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<SampleEntity> SampleEntitys { get; set; }
    }
}
