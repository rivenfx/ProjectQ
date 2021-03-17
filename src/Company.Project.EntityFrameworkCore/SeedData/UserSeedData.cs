using Riven.Data;
using Riven.Uow.Providers;
using Riven;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riven.MultiTenancy;

namespace Company.Project.SeedData
{
    public class UserSeedData : IDataSeedExecutor
    {
        readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public async Task Run(DataSeedContext dataSeedContext)
        {
            //_currentUnitOfWorkProvider.Current.ChangeTenant(dataSeedContext.TenantName)
        }
    }
}
