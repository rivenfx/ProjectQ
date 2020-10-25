using Riven.Modular;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project
{

    [DependsOn(
      typeof(CompanyProjectDomainModule)
      )]
    public class CompanyProjectEntityFrameworkCorePostgreSQLModule : CompanyProjectEntityFrameworkCoreMigratorModule
    {

    }
}
