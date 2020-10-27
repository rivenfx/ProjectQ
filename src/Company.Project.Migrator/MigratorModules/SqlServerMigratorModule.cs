using Riven.Modular;

namespace Company.Project.MigratorModules
{
    [DependsOn(
        typeof(CompanyProjectEntityFrameworkCoreSqlServerModule)
        )]
    public class SqlServerMigratorModule: MigratorModuleBase
    {

    }
}
