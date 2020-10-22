using Company.Project.Authorization.AppClaims;

using Microsoft.Extensions.DependencyInjection;

using Riven.Application;
using Riven.Modular;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using Riven.Identity.Authorization;

namespace Company.Project
{
    /// <summary>
    /// 应用权限常量
    /// </summary>
    public static class AppClaimsConsts
    {
        public const string RootNode = "root";

        /// <summary>
        /// 用户
        /// </summary>
        public static class User
        {
            public const string Node = "user.node";
            public const string Query = "user.query";
            public const string Create = "user.create";
            public const string Edit = "user.edit";
            public const string Delete = "user.delete";
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static class Role
        {
            public const string Node = "role.node";
            public const string Query = "role.query";
            public const string Create = "role.create";
            public const string Edit = "role.edit";
            public const string Delete = "role.delete";
        }


        /// <summary>
        /// 注册 claims
        /// </summary>
        /// <param name="serviceProvider">注入容器</param>
        /// <returns></returns>
        public static IServiceProvider RegisterBasicClaims(this IServiceProvider serviceProvider)
        {
            var permissions = new List<PermissionTmp>();
            var permissionsDict = new Dictionary<string, ClaimsAuthorizeScope>();

            var claimsAuthorizeAttrType = typeof(ClaimsAuthorizeAttribute);

            var applicationServiceType = typeof(IApplicationService);

            var moduleManager = (ModuleManager)serviceProvider.GetRequiredService<IModuleManager>();

            foreach (var module in moduleManager.ModuleDescriptors)
            {
                var types = module.ModuleType.Assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    if (
                        type.IsAbstract
                        || type.IsInterface
                        || type.IsGenericType
                        || !type.IsClass
                        || type.GetInterface(nameof(IApplicationService)) == null
                        )
                    {
                        continue;
                    }

                    var methodInfos = type.GetMethods();
                    foreach (var methodInfo in methodInfos)
                    {
                        var attrs = methodInfo
                            .GetCustomAttributes(claimsAuthorizeAttrType, false);
                        if (attrs.Length > 0)
                        {
                            var claimsAuthorizeAttr = attrs[0] as ClaimsAuthorizeAttribute;

                            foreach (var claim in claimsAuthorizeAttr.Claims)
                            {
                                if (permissionsDict.ContainsKey(claim))
                                {
                                    continue;
                                }
                                permissionsDict[claim] = claimsAuthorizeAttr.Scope;

                                permissions.Add(
                                    new PermissionTmp(claim, claimsAuthorizeAttr.Scope)
                                    );
                            }
                        }
                    }
                }
            }

            var groupClaims = permissions.GroupBy(o => o.Scope)
                .Select(o =>
                {
                    return new
                    {
                        Scope = o.Key,
                        Claims = o.AsQueryable().ToList()
                    };
                })
                .ToList();

            foreach (var groupClaim in groupClaims)
            {
                var dict = new Dictionary<ClaimItem, string>();
                foreach (var claim in groupClaim.Claims)
                {
                    foreach (var claimItem in ClaimsToTree(claim.Name, groupClaim.Scope, AppClaimsConsts.RootNode))
                    {
                        if (dict.ContainsKey(claimItem))
                        {
                            continue;
                        }

                        dict.Add(claimItem, string.Empty);
                    }
                }
            }


            var claimsManager = serviceProvider.GetRequiredService<IClaimsManager>();
            claimsManager.Add(CommonClaims());
            claimsManager.Add(HostClaims());
            claimsManager.Add(TenantClaims());

            return serviceProvider;
        }

        private static ClaimItem[] CommonClaims()
        {
            return new ClaimItem[]
            {
                ClaimItem.CreateWithCommon(RootNode),

                ClaimItem.CreateWithCommon(User.Node, RootNode),
                ClaimItem.CreateWithCommon(User.Query, User.Node),
                ClaimItem.CreateWithCommon(User.Create, User.Node),
                ClaimItem.CreateWithCommon(User.Edit,User.Node),
                ClaimItem.CreateWithCommon(User.Delete, User.Node),

                ClaimItem.CreateWithCommon(Role.Node,RootNode),
                ClaimItem.CreateWithCommon(Role.Query,Role.Node),
                ClaimItem.CreateWithCommon(Role.Create,Role.Node),
                ClaimItem.CreateWithCommon(Role.Edit,Role.Node),
                ClaimItem.CreateWithCommon(Role.Delete,Role.Node),
            };
        }

        private static ClaimItem[] HostClaims()
        {
            return new ClaimItem[]
            {

            };
        }

        private static ClaimItem[] TenantClaims()
        {
            return new ClaimItem[]
            {

            };
        }


        static List<ClaimItem> ClaimsToTree(string code, ClaimsAuthorizeScope scope, string defaultParent = null)
        {
            var result = new List<ClaimItem>();
            var claimArray = ClaimToArray(code).Reverse<string>().ToList();
            if (claimArray.Count == 0)
            {
                return result;
            }


            var maxCount = claimArray.Count;
            var index = 0;
            var parentIndex = index + 1;
            while (true)
            {
                var item = claimArray[index];
                if (maxCount == parentIndex)
                {
                    result.Add(
                        new ClaimItem(item, scope, defaultParent)
                        );
                    break;
                }

                result.Add(
                       new ClaimItem(item, scope, claimArray[parentIndex])
                       );


                index++;
                parentIndex = index + 1;
            }


            return result;
        }

        static List<string> ClaimToArray(string code)
        {
            var result = new List<string>();

            var index = 0;
            while (true)
            {
                index = code.IndexOf(".", index);
                if (index == -1)
                {
                    result.Add(code);
                    break;
                }

                var subStr = code.Substring(0, index);
                result.Add(subStr);
                index += 1;
            }

            return result;
        }

    }


    public class PermissionTmp
    {
        public string Name { get; private set; }

        public ClaimsAuthorizeScope Scope { get; private set; }



        public PermissionTmp(string name, ClaimsAuthorizeScope scope)
        {
            Name = name;
            Scope = scope;
        }
    }
}
