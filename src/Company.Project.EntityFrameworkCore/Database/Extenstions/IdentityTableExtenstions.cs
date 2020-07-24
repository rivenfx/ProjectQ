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

                // 移除索引
                var nameIndex = entityBuilder.HasIndex(o => o.NormalizedName).Metadata;
                entityBuilder.Metadata.RemoveIndex(nameIndex.Properties);

                // 创建复合索引
                entityBuilder.HasIndex(o => new { o.NormalizedName, o.TenantName }).IsUnique();
            });
            modelBuilder.Entity<RoleClaim>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(RoleClaim)}s");
            });

            modelBuilder.Entity<User>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(User)}s");
                entityBuilder.HasIndex(o => new { o.Nickname, o.TenantName })
                        .IsUnique();

                // 移除索引
                var nameIndex = entityBuilder.HasIndex(o => o.NormalizedUserName).Metadata;
                var emailIndex = entityBuilder.HasIndex(o => o.NormalizedEmail).Metadata;
                entityBuilder.Metadata.RemoveIndex(nameIndex.Properties);
                entityBuilder.Metadata.RemoveIndex(emailIndex.Properties);

                // 创建复合索引
                entityBuilder.HasIndex(o => new
                {
                    o.NormalizedUserName,
                    o.NormalizedEmail,
                    o.TenantName
                })
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
