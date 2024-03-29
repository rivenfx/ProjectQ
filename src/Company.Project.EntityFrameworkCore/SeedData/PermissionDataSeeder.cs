using Company.Project.Authorization.Permissions;

using Riven.Data;
using Riven.Identity.Permissions;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riven;

namespace Company.Project.SeedData
{
    public class PermissionDataSeeder : IDataSeedExecutor
    {
        readonly PermissionManager _permissionManager;

        public PermissionDataSeeder(PermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task Run(DataSeedContext dataSeedContext)
        {
            var tmpPermission = string.Empty;

            // 新增的权限字典
            var newPermissionDict = new Dictionary<string, int>();

            // 当前系统所有权限
            var systemPermissions = _permissionManager.GetSystemItem().Select(o => o.Name);

            // admin 角色拥有的权限
            var rolePermissionDict = (await _permissionManager.FindPermissions(IdentityPermissionType.Role, AppConsts.Authorization.SystemRoleName))
                .ToDictionary(o => o, o => string.Empty);


            // 新增权限
            foreach (var name in systemPermissions)
            {
                if (!rolePermissionDict.TryGetValue(name, out tmpPermission))
                {
                    rolePermissionDict[name] = string.Empty;
                    await _permissionManager.CreateAsync(new Permission()
                    {
                        Id = Guid.NewGuid().ToString("N").Replace("-", string.Empty),
                        Name = name,
                        Type = IdentityPermissionType.Role,
                        Provider = AppConsts.Authorization.SystemRoleName,
                        TenantName = dataSeedContext.TenantName
                    });
                }
            }

            // 取差集,获取需要删除的权限
            var deletePermissions = rolePermissionDict.Keys.Except(systemPermissions);
            if (deletePermissions.Count() > 0)
            {

                await _permissionManager.Remove(deletePermissions.ToArray());
            }
        }
    }
}
