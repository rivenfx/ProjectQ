using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Common.PageFilter.Dtos
{
    public class PageFilterItemDto
    {
        /// <summary>
        /// 组件类型(组件名称)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  标识名称(映射字段)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 操作条件 
        /// == 等于
        /// != 不等于
        /// > 大于
        /// < 小于
        /// >=  大于等于
        /// <= 小于
        /// range 范围
        /// in 在xx之中
        /// startswith 以xx开头
        /// endswith 已xx结尾
        /// contains 包含
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 必填校验
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 默认携带参数
        /// </summary>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        /// 值发生改变通知的page-filter组件名称集合
        /// </summary>
        public List<string> ValueChange { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 高级条件
        /// </summary>
        public bool Advanced { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }


        #region 扩展宽度

        /// <summary>
        ///  宽度, 当屏幕宽度: <576px, 一行24
        /// </summary>
        public int? XsWidth { get; set; }
        /// <summary>
        /// 宽度 当屏幕宽度: ≥576px, 一行24
        /// </summary>
        public int? SmWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥768px, 一行24
        /// </summary>
        public int? MdWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥992px, 一行24
        /// </summary>
        public int? LgWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥1200px, 一行24
        /// </summary>
        public int? XlWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥1600px, 一行24
        /// </summary>
        public int? XxlWidth { get; set; }

        #endregion
    }
}
