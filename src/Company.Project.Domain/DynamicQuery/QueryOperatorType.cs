using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.DynamicQuery
{
    /// <summary>
    /// 查询操作符类型
    /// </summary>
    public enum QueryOperatorType
    {
        /// <summary>
        /// 以xx开头
        /// </summary>
        StartsWith = 0,
        /// <summary>
        /// 以xx结尾
        /// </summary>
        EndsWith = 1,

        /// <summary>
        /// 等于
        /// </summary>
        Equal = 2,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 3,

        /// <summary>
        /// 大于
        /// </summary>
        Greater = 4,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual = 5,

        /// <summary>
        /// 小于
        /// </summary>
        Less = 6,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual = 7,

        /// <summary>
        /// 在aa与bb之间
        /// </summary>
        Between = 8,
        /// <summary>
        /// 存在于 xxx 中
        /// </summary>
        In = 9,

        /// <summary>
        /// 包含
        /// </summary>
        Contains = 10

    }
}
