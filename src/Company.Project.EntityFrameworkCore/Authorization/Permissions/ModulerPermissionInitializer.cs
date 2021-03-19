using Riven.Authorization;
using Riven.Identity.Permissions;
using Riven.Modular;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public class ModulerPermissionInitializer : PermissionInitializer
    {
        readonly IModuleManager _moduleManager;

        public ModulerPermissionInitializer(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        protected override IEnumerable<IPermissionAuthorizeAttribute> GetPermissionAuthorizeAttributes()
        {
            // 模块信息
            var moduleDescriptors = (_moduleManager as ModuleManager)?.ModuleDescriptors;
            if (moduleDescriptors == null)
            {
                throw new Exception("not found module descriptors");
            }
            var assemblies = moduleDescriptors.Select(o => o.ModuleType.Assembly).ToArray();

            return this.GetPermissionAuthorizeAttributes(assemblies);
        }
    }
}
