using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Dtos
{
    /// <summary>
    /// 查询输入
    /// </summary>
    public class QueryInput
    {
        public int PageSize { get; set; }

        public int SkipCount { get; set; }
    }
}
