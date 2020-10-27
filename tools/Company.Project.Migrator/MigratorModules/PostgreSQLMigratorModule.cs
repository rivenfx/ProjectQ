using Riven.Modular;

namespace Company.Project.MigratorModules
{
    [DependsOn(
        typeof(CompanyProjectEntityFrameworkCorePostgreSQLModule)
        )]
    public class PostgreSQLMigratorModule : MigratorModuleBase
    {

    }
}
