using Company.Project.Authorization.Tokens;
using Company.Project.MultiTenancy;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Mapster;
using System.Text.Json;

namespace Company.Project.Database.Extensions
{
    public static class AppTokenTableExtensions
    {
        /// <summary>
        /// App Token配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ConfiurationAppTokenTable(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppToken>((b) =>
            {
                b.Property(o => o.Value)
                    .HasMaxLength(64)
                    .IsRequired()
                    ;

                b.Property(o => o.Scheme)
                    .HasMaxLength(64)
                    .IsRequired()
                    ;

                b.Property(o => o.Type)
                    .HasMaxLength(64)
                    .IsRequired()
                    .HasConversion<string>()
                    ;

                b.Property(o => o.UserId)
                    .HasMaxLength(128)
                    .IsRequired()
                    ;

                b.Property(o => o.Claims)
                    .HasMaxLength(1024)
                    .IsRequired()
                    .HasConversion<string>(
                        s => JsonSerializer.Serialize(s.Adapt<List<ClaimLite>>(), null),
                        t => JsonSerializer.Deserialize<List<ClaimLite>>(t, null).Adapt<List<Claim>>()
                    )
                    ;

                b.Property(o => o.CreateTime)
                    .IsRequired();

                b.Property(o => o.Expiration)
                    .IsRequired();

                b.Property(o => o.TenantName)
                    .HasMaxLength(64);
            });

            return modelBuilder;
        }
    }
}
