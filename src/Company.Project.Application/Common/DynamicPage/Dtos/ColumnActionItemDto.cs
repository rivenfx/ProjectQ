namespace Company.Project.Common.DynamicPage.Dtos
{
    /// <summary>
    /// 操作项
    /// </summary>
    public class ColumnActionItemDto
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 操作控件类型
        /// </summary>
        public ColumnControl Type { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Acl { get; set; }


        /// <summary>
        /// 子级操作按钮 当 <see cref="Type"/> 值为 <see cref="ColumnControl.Select"/> 时使用
        /// </summary>
        public ColumnActionItemDto[] Buttons { get; set; }

    }

}
