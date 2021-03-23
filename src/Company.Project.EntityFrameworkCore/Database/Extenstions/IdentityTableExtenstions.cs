using Company.Project.Authorization.Permissions;
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
            modelBuilder.Entity<Permission>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(Permission)}s");
            });

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
            modelBuilder.Entity<UserLogin>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserLogin)}s");

                // 移除复合主键
                var userRoleKey = entityBuilder.HasKey(o => new { o.LoginProvider, o.ProviderKey }).Metadata;
                entityBuilder.Metadata.RemoveIndex(userRoleKey.Properties);

                // 创建新的复合主键
                entityBuilder.HasKey(o => new { o.LoginProvider, o.ProviderKey, o.TenantName });
            });
            modelBuilder.Entity<UserToken>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserToken)}s");

                // 移除复合主键
                var userRoleKey = entityBuilder.HasKey(o => new { o.UserId, o.LoginProvider, o.Name }).Metadata;
                entityBuilder.Metadata.RemoveIndex(userRoleKey.Properties);

                // 创建主键
                entityBuilder.HasKey(o => o.Id);

                // 创建新的索引
                entityBuilder.HasIndex(o => new { o.UserId, o.LoginProvider, o.Name, o.TenantName }).IsUnique();
            });
            modelBuilder.Entity<UserRole>((entityBuilder) =>
            {
                entityBuilder.ToTable($"{nameof(UserRole)}s");

                // 移除复合主键
                var userRoleKey = entityBuilder.HasKey(o => new { o.UserId, o.RoleId }).Metadata;
                entityBuilder.Metadata.RemoveIndex(userRoleKey.Properties);

                // 创建主键
                entityBuilder.HasKey(o => o.Id);

                // 创建新的索引
                entityBuilder.HasIndex(o => new { o.UserId, o.RoleId, o.TenantName }).IsUnique();
            });

            return modelBuilder;
        }
    }
}
