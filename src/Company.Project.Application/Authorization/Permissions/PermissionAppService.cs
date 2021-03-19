using Riven.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riven;
using Company.Project.Authorization.Permissions.Dtos;
using Riven.Authorization;

namespace Company.Project.Authorization.Permissions
{
    [PermissionAuthorize]
    public class PermissionAppService : IApplicationService
    {
        readonly IAppSession _appSession;
        readonly IPermissionManager _permissionManager;

        public PermissionAppService(IAppSession appSession, IPermissionManager permissionManager)
        {
            _appSession = appSession;
            _permissionManager = permissionManager;
        }

        public async Task<List<string>> GetAllPermissions()
        {
            await Task.Yield();


            return _permissionManager.GetAll().Select(o => o.Name).ToList();
        }

        public async Task<List<PermissionItemDto>> GetAllPermissionsWithTree()
        {
            await Task.Yield();


            return _permissionManager.GetAll().ProjectTo<PermissionItemDto>().ToList();
        }

    }
}
