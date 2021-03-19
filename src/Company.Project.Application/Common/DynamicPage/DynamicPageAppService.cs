using Company.Project.Common.DynamicPage.Dtos;
using Company.Project.Dtos;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Riven.Application;
using Riven.Authorization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Common.DynamicPage
{
    [PermissionAuthorize]
    public class DynamicPageAppService : IApplicationService
    {

        static DynamicPageDto _defaultDynamicPageInfoResult = new DynamicPageDto();

        static ListResultDto<PageFilterItemDto> _defaultPageFilterResult = new ListResultDto<PageFilterItemDto>(
                new List<PageFilterItemDto>()
            );

        static ListResultDto<ColumnItemDto> _defaultColumnResult = new ListResultDto<ColumnItemDto>(
              new List<ColumnItemDto>()
          );

        static List<PageFilterItemDto> _defaultPageFilterList = new List<PageFilterItemDto>();
        static List<ColumnItemDto> _defaultColumnList = new List<ColumnItemDto>();


        readonly IHostEnvironment _hostEnv;

        readonly ILogger<DynamicPageAppService> _logger;

        public DynamicPageAppService(IHostEnvironment hostEnv, ILogger<DynamicPageAppService> logger)
        {
            _hostEnv = hostEnv;
            _logger = logger;
        }


        public async Task<DynamicPageDto> GetDynamicPageInfo(string name)
        {
            try
            {
                var pageFilters = await this.GetPageFiltersFromFile(name);
                var columns = await this.GetColumnsFromFile(name);

                return new DynamicPageDto()
                {
                    PageFilters = pageFilters,
                    Columns = columns
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return _defaultDynamicPageInfoResult;
            }
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<PageFilterItemDto>> GetPageFilters(string name)
        {
            var pageFilters = await this.GetPageFiltersFromFile(name);
            if (pageFilters.Count == 0)
            {
                return _defaultPageFilterResult;
            }

            return new ListResultDto<PageFilterItemDto>(pageFilters);
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<ColumnItemDto>> GetColumns(string name)
        {
            var columns = await this.GetColumnsFromFile(name);
            if (columns.Count == 0)
            {
                return _defaultColumnResult;
            }

            return new ListResultDto<ColumnItemDto>(columns);
        }


        protected async Task<List<PageFilterItemDto>> GetPageFiltersFromFile(string name)
        {
            var configFilePath = Path.Join(
                _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "page-filter",
                    $"{name}.json"
                );

            if (!File.Exists(configFilePath))
            {
                return _defaultPageFilterList;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var pageFilters = JsonConvert.DeserializeObject<List<PageFilterItemDto>>(fileContent);

            return pageFilters;
        }

        protected async Task<List<ColumnItemDto>> GetColumnsFromFile(string name)
        {
            var configFilePath = Path.Join(
                    _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "list-view",
                    $"{name}.json"
                );
            if (!File.Exists(configFilePath))
            {
                return _defaultColumnList;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var listView = JsonConvert.DeserializeObject<List<ColumnItemDto>>(fileContent);

            return listView;
        }
    }
}
