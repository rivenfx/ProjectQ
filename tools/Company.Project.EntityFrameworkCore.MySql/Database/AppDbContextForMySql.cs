using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Database
{
    public class AppDbContextForMySql : AppDbContext
    {
        public AppDbContextForMySql(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {
        }

    }
}
