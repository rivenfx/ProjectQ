using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.DynamicQuery
{
    /// <summary>
    /// 排序表达式依据实体
    /// </summary>
    public class SortCondition
    {
        /// <summary>
        /// 排序字段名称
        /// xxx
        /// xxx.xxx
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 优先顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 排序类别
        /// </summary>
        public SortType Operator { get; set; }
    }
}
