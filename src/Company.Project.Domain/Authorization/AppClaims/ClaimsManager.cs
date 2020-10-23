using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Riven.Dependency;
using Riven.Identity.Authorization;
using Riven.Modular;

namespace Company.Project.Authorization.AppClaims
{
    public class ClaimsManager : IClaimsManager, ISingletonDependency
    {
        bool _inited;
        List<ClaimItem> _claims = new List<ClaimItem>();

        readonly IServiceProvider _serviceProvider;

        public ClaimsManager(IServiceProvider serviceProvider)
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

        public void Add(params ClaimItem[] claims)
        {
            if (claims == null || claims.Length == 0)
            {
                return;
            }
            _claims = _claims.Union(claims).Distinct().ToList();
        }

        public void Clear()
        {
            _claims.Clear();
        }

        public IQueryable<ClaimItem> GetAll()
        {
            // 未开启多租户只返回公共的
            if (!Riven.MultiTenancy.MultiTenancyConfig.IsEnabled)
            {
                return _claims.Where(o => o.Scope == ClaimsAuthorizeScope.Common)
                            .OrderBy(o => o.Sort)
                            .AsQueryable();
            }

            // 开启多租户则返回所有的
            return _claims
                .OrderBy(o => o.Sort)
                .AsQueryable();
        }



        public void Remove(params ClaimItem[] claims)
        {
            if (claims == null || claims.Length == 0)
            {
                return;
            }
            _claims = _claims.Except(claims).ToList();
        }

        #region 从模块初始化 Claims

        /// <summary>
        /// 根据模块初始化权限
        /// </summary>
        /// <param name="moduleManager">模块管理器</param>
        /// <param name="claimsManager">claim管理器</param>
        static void InitForModules(IModuleManager moduleManager, IClaimsManager claimsManager)
        {
            // 反射使用的几个类型
            var claimsAuthorizeAttrType = typeof(ClaimsAuthorizeAttribute);
            var controllerType = typeof(Microsoft.AspNetCore.Mvc.ControllerBase);
            var applicationServiceName = nameof(Riven.Application.IApplicationService);

            // claim 字典集合
            var claimDict = new Dictionary<ClaimInfo, ClaimsAuthorizeScope>();


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
                        // 获取方法上的 ClaimsAuthorizeAttribute 特性
                        var attrs = methodInfo
                            .GetCustomAttributes(claimsAuthorizeAttrType, false);
                        if (attrs.Length == 0)
                        {
                            continue;
                        }

                        // 将特性中的Claim数据加入字典
                        var claimsAuthorizeAttr = attrs[0] as ClaimsAuthorizeAttribute;
                        var newItem = default(ClaimInfo);
                        foreach (var claim in claimsAuthorizeAttr.Claims)
                        {
                            newItem = new ClaimInfo(claim, claimsAuthorizeAttr.Scope);
                            if (claimDict.ContainsKey(newItem))
                            {
                                continue;
                            }
                            claimDict[newItem] = claimsAuthorizeAttr.Scope;
                        }
                    }
                }
            }

            // 将数据进行分组
            var groupClaims = claimDict.GroupBy(o => o.Value)
                .Select(o =>
                {
                    return new
                    {
                        Scope = o.Key,
                        Claims = o.AsEnumerable().Select(p => p.Key)
                    };
                })
                .ToList();

            // 遍历分组后的数据并生成结构添加到管理器
            var claimItemDict = new Dictionary<ClaimItem, string>();
            claimItemDict.Add(new ClaimItem(AppClaimsConsts.RootNode, null, ClaimsAuthorizeScope.Common), string.Empty);
            foreach (var groupClaim in groupClaims)
            {
                foreach (var claim in groupClaim.Claims)
                {
                    foreach (var claimItem in ClaimsToTree(claim.Name, groupClaim.Scope, AppClaimsConsts.RootNode))
                    {
                        if (claimItemDict.ContainsKey(claimItem))
                        {
                            continue;
                        }

                        claimItemDict.Add(claimItem, string.Empty);
                    }
                }
                claimsManager.Add(claimItemDict.Keys.ToArray());
                claimItemDict.Clear();
            }
        }

        /// <summary>
        /// claim 转树形数据结构
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="scope"></param>
        /// <param name="defaultParent"></param>
        /// <returns></returns>
        static List<ClaimItem> ClaimsToTree(string claim, ClaimsAuthorizeScope scope, string defaultParent = null)
        {
            var result = new List<ClaimItem>();
            var claimArray = ClaimToArray(claim).Reverse<string>().ToList();
            if (claimArray.Count == 0)
            {
                return result;
            }


            var maxCount = claimArray.Count;
            var index = 0;
            var parentIndex = index + 1;
            while (true)
            {
                var newClaim = claimArray[index];
                if (maxCount == parentIndex)
                {
                    result.Add(
                        new ClaimItem(newClaim, defaultParent, scope)
                        );
                    break;
                }

                result.Add(
                       new ClaimItem(newClaim, claimArray[parentIndex], scope)
                       );

                index++;
                parentIndex = index + 1;
            }


            return result;
        }

        /// <summary>
        /// claim 转数组，如 "user.create" 输出 ["user","user.create"]
        /// </summary>
        /// <param name="claim">claim</param>
        /// <returns></returns>
        static List<string> ClaimToArray(string claim)
        {
            var result = new List<string>();

            var index = 0;
            while (true)
            {
                index = claim.IndexOf(".", index);
                if (index == -1)
                {
                    result.Add(claim);
                    break;
                }

                var subStr = claim.Substring(0, index);
                result.Add(subStr);
                index += 1;
            }

            return result;
        }

        protected class ClaimInfo
        {
            public string Name { get; private set; }

            public ClaimsAuthorizeScope Scope { get; private set; }



            public ClaimInfo(string name, ClaimsAuthorizeScope scope)
            {
                Name = name;
                Scope = scope;
            }
        }

        #endregion
    }

}
