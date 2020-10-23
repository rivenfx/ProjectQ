using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Company.Project.Authorization.Permissions
{
    public interface IPermissionManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IQueryable<PermissionItem> GetAll();


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="claims"></param>
        void Add(params PermissionItem[] claims);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="claims"></param>
        void Remove(params PermissionItem[] claims);


        /// <summary>
        /// 清空所有
        /// </summary>
        void Clear();

      
    }
}
