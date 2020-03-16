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

namespace Company.Project.SampleEntitys
{
    public class SampleEntityAppService : IApplicationService
    {
        readonly IRepository<SampleEntity, long> _repository;
        readonly IUnitOfWorkManager _unitOfWorkManager;

        public SampleEntityAppService(IRepository<SampleEntity, long> repository, IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<SampleEntity>> GetAll()
        {
            var currentUnitOfWork = this._unitOfWorkManager.Current;

            // 切换数据库连接字符串
            //using (currentUnitOfWork.SetConnectionStringName("TenantA"))
            //{
            //    var resultWithTenantA = await this._repository.GetAll().ToListAsync();
            //}

            // 切换 DbContext
            //using (currentUnitOfWork.SetDbContextProvider("TenantA"))
            //{
            //    var resultWithTenantA = await this._repository.GetAll().ToListAsync();
            //}

            return await _repository.GetAll().ToListAsync();
        }

    }
}
