using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.DynamicQuery
{
    /// <summary>
    /// 查询表达式依据实体
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 字段名称 
        /// xxx
        /// xxx.xxx
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 该字段在数据库中不可为空
        /// </summary>
        public bool NotNull { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public QueryOperatorType Operator { get; set; }

    }
}
