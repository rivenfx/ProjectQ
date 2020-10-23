using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Riven.Dependency;
using Riven.Identity.Authorization;
using Riven.Modular;

namespace Company.Project.Authorization.Permissions
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        bool _inited;
        List<PermissionItem> _permissions = new List<PermissionItem>();

        readonly IServiceProvider _serviceProvider;

        public PermissionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Init()
        {
            if (this._inited)
            {
                return;
            }
            this._inited = true;
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var moduleManager = serviceScope.ServiceProvider.GetRequiredService<IModuleManager>();
                InitForModules(moduleManager, this);
            }
        }

        public void Add(params PermissionItem[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return;
            }
            _permissions = _permissions.Union(permissions).Distinct().ToList();
        }

        public void Clear()
        {
            _permissions.Clear();
        }

        public IQueryable<PermissionItem> GetAll()
        {
            // 未开启多租户只返回公共的
            if (!Riven.MultiTenancy.MultiTenancyConfig.IsEnabled)
            {
                return _permissions.Where(o => o.Scope == PermissionAuthorizeScope.Common)
                            .OrderBy(o => o.Sort)
                            .AsQueryable();
            }

            // 开启多租户则返回所有的
            return _permissions
                .OrderBy(o => o.Sort)
                .AsQueryable();
        }



        public void Remove(params PermissionItem[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return;
            }
            _permissions = _permissions.Except(permissions).ToList();
        }

        #region 从模块初始化 Permissions

        /// <summary>
        /// 根据模块初始化权限
        /// </summary>
        /// <param name="moduleManager">模块管理器</param>
        /// <param name="permissionManager">claim管理器</param>
        static void InitForModules(IModuleManager moduleManager, IPermissionManager permissionManager)
        {
            // 反射使用的几个类型
            var claimsAuthorizeAttrType = typeof(PermissionAuthorizeAttribute);
            var controllerType = typeof(Microsoft.AspNetCore.Mvc.ControllerBase);
            var applicationServiceName = nameof(Riven.Application.IApplicationService);

            // Permission 字典集合
            var permissionDict = new Dictionary<PermissionInfo, PermissionAuthorizeScope>();


            // 模块信息
            var moduleDescriptors = (moduleManager as ModuleManager)?.ModuleDescriptors;
            if (moduleDescriptors == null)
            {
                throw new Exception("not found module descriptors");
            }

            // 扫描所有模块中导出的类型
            foreach (var module in moduleDescriptors)
            {
                var types = module.ModuleType.Assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    // 不符合条件的
                    if (
                        type.IsAbstract
                        || type.IsInterface
                        || type.IsGenericType
                        || !type.IsClass
                        || (type.GetInterface(applicationServiceName) == null && !type.IsSubclassOf(controllerType))
                        )
                    {
                        continue;
                    }


                    // 获取类型中所有的函数并遍历
                    var methodInfos = type.GetMethods();
                    foreach (var methodInfo in methodInfos)
                    {
                        // 获取方法上的 PermissionAuthorizeAttribute 特性
                        var attrs = methodInfo
                            .GetCustomAttributes(claimsAuthorizeAttrType, false);
                        if (attrs.Length == 0)
                        {
                            continue;
                        }

                        // 将特性中的 Permission 数据加入字典
                        var permissionAuthorizeAttr = attrs[0] as PermissionAuthorizeAttribute;
                        var newItem = default(PermissionInfo);
                        foreach (var permission in permissionAuthorizeAttr.Permissions)
                        {
                            newItem = new PermissionInfo(permission, permissionAuthorizeAttr.Scope);
                            if (permissionDict.ContainsKey(newItem))
                            {
                                continue;
                            }
                            permissionDict[newItem] = permissionAuthorizeAttr.Scope;
                        }
                    }
                }
            }

            // 将数据进行分组
            var groupPermissions = permissionDict.GroupBy(o => o.Value)
                .Select(o =>
                {
                    return new
                    {
                        Scope = o.Key,
                        Permissions = o.AsEnumerable().Select(p => p.Key)
                    };
                })
                .ToList();

            // 遍历分组后的数据并生成结构添加到管理器
            var permissionItemDict = new Dictionary<PermissionItem, string>();
            permissionItemDict.Add(new PermissionItem(AppPermissions.RootNode, null, PermissionAuthorizeScope.Common), string.Empty);
            foreach (var groupPermission in groupPermissions)
            {
                foreach (var permission in groupPermission.Permissions)
                {
                    foreach (var permissionItem in PermissionToTree(permission.Name, groupPermission.Scope, AppPermissions.RootNode))
                    {
                        if (permissionItemDict.ContainsKey(permissionItem))
                        {
                            continue;
                        }

                        permissionItemDict.Add(permissionItem, string.Empty);
                    }
                }
                permissionManager.Add(permissionItemDict.Keys.ToArray());
                permissionItemDict.Clear();
            }
        }

        /// <summary>
        /// Permission 转树形数据结构
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="scope"></param>
        /// <param name="defaultParent"></param>
        /// <returns></returns>
        static List<PermissionItem> PermissionToTree(string permission, PermissionAuthorizeScope scope, string defaultParent = null)
        {
            var result = new List<PermissionItem>();
            var permissionArray = PermissionToArray(permission).Reverse<string>().ToList();
            if (permissionArray.Count == 0)
            {
                return result;
            }


            var maxCount = permissionArray.Count;
            var index = 0;
            var parentIndex = index + 1;
            while (true)
            {
                var newPermission = permissionArray[index];
                if (maxCount == parentIndex)
                {
                    result.Add(
                        new PermissionItem(newPermission, defaultParent, scope)
                        );
                    break;
                }

                result.Add(
                       new PermissionItem(newPermission, permissionArray[parentIndex], scope)
                       );

                index++;
                parentIndex = index + 1;
            }


            return result;
        }

        /// <summary>
        /// Permission 转数组，如 "user.create" 输出 ["user","user.create"]
        /// </summary>
        /// <param name="permission">claim</param>
        /// <returns></returns>
        static List<string> PermissionToArray(string permission)
        {
            var result = new List<string>();

            var index = 0;
            while (true)
            {
                index = permission.IndexOf(".", index);
                if (index == -1)
                {
                    result.Add(permission);
                    break;
                }

                var subStr = permission.Substring(0, index);
                result.Add(subStr);
                index += 1;
            }

            return result;
        }

        protected class PermissionInfo
        {
            public string Name { get; private set; }

            public PermissionAuthorizeScope Scope { get; private set; }



            public PermissionInfo(string name, PermissionAuthorizeScope scope)
            {
                Name = name;
                Scope = scope;
            }
        }

        #endregion
    }

}
