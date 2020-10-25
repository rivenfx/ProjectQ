using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using Company.Project.Configuration;

namespace Company.Project.Database
{
    public class AppDesignTimeDbContextFactoryForMySql : DesignTimeDbContextFactoryBase<AppDbContextForMySql>
    {

    }
}
