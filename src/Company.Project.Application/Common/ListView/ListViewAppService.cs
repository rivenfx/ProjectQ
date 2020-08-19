using Company.Project.Common.ListView.Dtos;
using Company.Project.Dtos;

using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Riven.Application;
using Riven.Identity.Authorization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Common.ListView
{
    [ClaimsAuthorize]
    public class ListViewAppService : IApplicationService
    {
        static ListResultDto<ColumnItemDto> _defaultResult = new ListResultDto<ColumnItemDto>(
               new List<ColumnItemDto>()
           );

        readonly IHostEnvironment _hostEnv;

        public ListViewAppService(IHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv;
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<ColumnItemDto>> GetListView(string name)
        {
            var configFilePath = Path.Join(_hostEnv.ContentRootPath, "wwwroot", "configs", "list-view", $"{name}.json");
            if (!File.Exists(configFilePath))
            {
                return _defaultResult;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var pageFilters = JsonConvert.DeserializeObject<List<ColumnItemDto>>(fileContent);

            return new ListResultDto<ColumnItemDto>(pageFilters);
        }
    }
}
