using Company.Project.Samples;
using Riven.Application;
using Riven.Repositories;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riven.Extensions;
using Microsoft.AspNetCore.Authorization;
using Riven.Identity.Authorization;

namespace Company.Project.SampleEntitys
{
    public class SampleEntityAppService : IApplicationService
    {
        readonly IRepository<SampleEntity> _repository;
        readonly IUnitOfWorkManager _unitOfWorkManager;

        IActiveUnitOfWork CurrentUnitOfWork => this._unitOfWorkManager.Current;

        public SampleEntityAppService(IRepository<SampleEntity> repository, IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        //[UnitOfWork(ConnectionStringName = "TenantB")]
        public async Task<List<SampleEntity>> GetAll()
        {
            //#region 切换数据库连接字符串/切换DbContext 用例

            //// 切换数据库连接字符串
            //using (CurrentUnitOfWork.SetConnectionStringName("TenantA"))
            //{
            //    var resultWithTenantA = await this._repository.GetAll().ToListAsync();
            //}

            //// 切换 DbContext
            //using (CurrentUnitOfWork.SetDbContextProvider("TenantA"))
            //{
            //    var resultWithTenantA = await this._repository.GetAll().ToListAsync();
            //}

            //#endregion


            var resultWithTenantB = await _repository.GetAll().ToListAsync();

            return resultWithTenantB;
        }

        [ClaimsAuthorize("SampleEntity.Create")]
        public async Task<bool> Create(string name)
        {
            await _repository.InsertAsync(new SampleEntity()
            {
                DisplayName = name
            });

            return true;
        }

        [Authorize]
        public async Task<bool> Delete(long id)
        {
            await _repository.DeleteAsync(id);

            return true;
        }
    }
}
