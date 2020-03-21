using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = "server=(localdb)\\MSSQLLocalDB;database=RivenTest;uid=sa;pwd=123;";


            var builder = new DbContextOptionsBuilder<AppDbContext>();

            AppDbContextConfigurer.Configure(builder, connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
