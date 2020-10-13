using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Common.DynamicPage.Dtos
{
    public class DynamicPageDto
    {
        public List<PageFilterItemDto> PageFilters { get; set; }

        public List<ColumnItemDto> Columns { get; set; }


        public DynamicPageDto()
        {

        }
    }
}
