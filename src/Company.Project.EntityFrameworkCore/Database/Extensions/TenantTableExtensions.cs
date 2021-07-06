using Company.Project.MultiTenancy;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database.Extensions
{
    public static class TenantTableExtensions
    {
        public static ModelBuilder ConfiurationTenantTable(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>((b) =>
            {
                b.HasIndex(o => o.Name)
                    .IsUnique();
            });

            return modelBuilder;
        }
    }
}
