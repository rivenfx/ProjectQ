using Company.Project.Common.PageFilter.Dtos;
using Company.Project.Dtos;

using Microsoft.Extensions.Hosting;

using Riven.Application;
using Riven.Identity.Authorization;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Company.Project.Common.PageFilter
{
    [ClaimsAuthorize]
    public class PageFilterAppService : IApplicationService
    {
        static ListResultDto<PageFilterItemDto> _defaultResult = new ListResultDto<PageFilterItemDto>(
                new List<PageFilterItemDto>()
            );

        readonly IHostEnvironment _hostEnv;

        public PageFilterAppService(IHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv;
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<PageFilterItemDto>> GetPageFilter(string name)
        {
            var configFilePath = Path.Join(_hostEnv.ContentRootPath, "wwwroot", "configs", "page-filter", $"{name}.json");
            if (!File.Exists(configFilePath))
            {
                return _defaultResult;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var pageFilters = JsonConvert.DeserializeObject<List<PageFilterItemDto>>(fileContent);

            return new ListResultDto<PageFilterItemDto>(pageFilters);
        }
    }
}
