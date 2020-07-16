using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Dtos
{
    public class ListResultDto<T>
    {
        public IReadOnlyList<T> Items { get; set; }

        public ListResultDto()
        {
            Items = default(IReadOnlyList<T>);
        }

        public ListResultDto(List<T> items)
            : base()
        {
            if (items != null)
            {
                Items = items.AsReadOnly();
            }
        }
    }
}
