using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Company.Project.Common.ListView.Dtos
{
    public class ColumnItemDto
    {
        public ColumnItemDto()
        {
            NumberDigits = 2;
            DateFormat = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 字段名,支持嵌套,aa.bb
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段类型,datetime,number
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 自定义渲染器名称
        /// </summary>
        public string Render { get; set; }

        /// <summary>
        /// 排序号,越小越靠前
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// 列表宽度
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 列类型为number时保留小数位
        /// </summary>
        public int NumberDigits { get; set; }

        /// <summary>
        /// 列类型为datetime类型格式化规则
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// 统计类型,当列为数字时生效
        /// </summary>
        public ColumnItemStatistical? Statistical { get; set; }

        /// <summary>
        /// 固定列, left 或 right, 指定 Width 时生效
        /// </summary>
        public ColumnItemFixed? Fixed { get; set; }


        /// <summary>
        /// 操作,当 <see cref="Type"/> 值为 action 时
        /// </summary>
        public ColumnActionItemDto[] Actions { get; set; }
    }

}
