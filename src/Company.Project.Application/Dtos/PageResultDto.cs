using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Dtos
{
    public class PageResultDto<T>
    {
        public IReadOnlyList<T> Items { get; set; }

        public long Total { get; set; }

        public PageResultDto()
        {
            this.Items = default(IReadOnlyList<T>);
            this.Total = 0;
        }

        public PageResultDto(List<T> items, long total)
            : base()
        {
            if (items != null)
            {
                this.Items = items.AsReadOnly();
            }
            if (total > 0)
            {
                this.Total = total;
            }
        }
    }
}
