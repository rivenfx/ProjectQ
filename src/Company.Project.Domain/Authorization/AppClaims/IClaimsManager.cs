using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Company.Project.Authorization.AppClaims
{
    public interface IClaimsManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IQueryable<ClaimItem> GetAll();


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="claims"></param>
        void Add(params ClaimItem[] claims);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="claims"></param>
        void Remove(params ClaimItem[] claims);


        /// <summary>
        /// 清空所有
        /// </summary>
        void Clear();

      
    }
}
