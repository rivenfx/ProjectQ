using Riven.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riven;
using Riven.Extensions;
using Company.Project.Authorization.Permissions.Dtos;
using Riven.Authorization;
using Riven.Identity.Permissions;

namespace Company.Project.Authorization.Permissions
{
    [PermissionAuthorize]
    public class PermissionAppService : IApplicationService
    {
        readonly IAppSession _appSession;
        readonly PermissionManager _permissionManager;

        public PermissionAppService(IAppSession appSession, PermissionManager permissionManager)
        {
            _appSession = appSession;
            _permissionManager = permissionManager;
        }

        public async Task<List<string>> GetAllPermissions()
        {
            await Task.Yield();


            return _permissionManager.GetSystemItem().Select(o => o.Name).ToList();
        }

        public async Task<List<PermissionItemDto>> GetAllPermissionsWithTree()
        {
            await Task.Yield();


            return _permissionManager.GetSystemItem().ProjectTo<PermissionItemDto>().ToList();
        }

    }
}
