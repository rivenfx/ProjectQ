using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContextForSqlServer : AppDbContext
    {
        public AppDbContextForSqlServer(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {
        }

    }
}
