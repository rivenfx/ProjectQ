using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContextForPostgreSQL : AppDbContext
    {
        public AppDbContextForPostgreSQL(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {
        }

    }
}
