using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Riven;
using Riven.Dependency;
using Riven.Repositories;

namespace Company.Project.MultiTenancy
{
    public interface ITenantManager : IDomainService<Tenant, Guid>
    {
        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="name">名称(唯一)</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="isStatic">是否为系统内置</param>
        /// <param name="isActive">是否激活</param>
        /// <returns></returns>
        Task<Tenant> Create(string name, string displayName, string description = null, string connectionString = null, bool isStatic = false, bool isActive = false);

        /// <summary>
        /// 根据租户名称获取租户
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<Tenant> GetByName(string name);

        /// <summary>
        /// 根据租户名称获取租户
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<string> GetDisplayNameByName(string name);

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="name">租户名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">明细</param>
        /// <returns></returns>
        Task<Tenant> Update(string name, string displayName, string description, bool isActive);

        /// <summary>
        /// 根据名称删除租户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Delete(string name);

        /// <summary>
        /// 加载租户连接字符串信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<IConnectionStringProvider> LoadConnectionStringProviders();
    }
}
