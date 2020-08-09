using Company.Project.Common.PageFilter.Dtos;
using Company.Project.Dtos;

using Riven.Application;
using Riven.Identity.Authorization;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Common.PageFilter
{
    [ClaimsAuthorize]
    public class PageFilterAppService : IApplicationService
    {

        public async Task<ListResultDto<PageFilterItemDto>> GetPageFilter(string name)
        {
            await Task.Yield();

            return new ListResultDto<PageFilterItemDto>(new List<PageFilterItemDto>());
        }
    }
}
