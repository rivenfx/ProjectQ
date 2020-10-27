using Riven.Modular;

namespace Company.Project.MigratorModules
{
    [DependsOn(
       typeof(CompanyProjectEntityFrameworkCoreMySqlModule)
       )]
    public class MySqlMigratorModule : MigratorModuleBase
    {

    }
}
