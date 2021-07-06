using Company.Project.MultiTenancy;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database.Extensions
{
    public static class TenantTableExtensions
    {
        /// <summary>
        /// 租户表配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ConfiurationTenantTable(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>((b) =>
            {
                b.HasIndex(o => o.Name)
                    .IsUnique();

                b.Property(o => o.Name)
                    .HasMaxLength(64);

                b.Property(o => o.DisplayName)
                    .HasMaxLength(128);

                b.Property(o => o.Description)
                    .HasMaxLength(512);

                b.Property(o => o.ConnectionString)
                    .HasMaxLength(1024);
            });

            return modelBuilder;
        }
    }
}
