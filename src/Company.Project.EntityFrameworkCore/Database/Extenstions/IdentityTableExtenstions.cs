using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database.Extenstions
{
    public static class IdentityTableExtenstions
    {
        /// <summary>
        /// 配置 asp.net core identity 表
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ConfiurationIdentityTables(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(Role)}s");
            });
            modelBuilder.Entity<RoleClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(RoleClaim)}s");
            });

            modelBuilder.Entity<User>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(User)}s");
                entityBuilder.HasIndex(o => o.Nickname)
                        .IsUnique();
            });
            modelBuilder.Entity<UserClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserClaim)}s");
            });
            modelBuilder.Entity<UserLogin>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserLogin)}s");
            });
            modelBuilder.Entity<UserToken>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserToken)}s");
            });
            modelBuilder.Entity<UserRole>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserRole)}s");
            });

            return modelBuilder;
        }
    }
}
