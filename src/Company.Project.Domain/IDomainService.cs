using Microsoft.EntityFrameworkCore;

using Riven.Dependency;
using Riven.Entities;
using Riven.Repositories;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project
{
    public interface IDomainService<TEntity, TPrimaryKey> : ITransientDependency
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IUnitOfWorkManager UnitOfWorkManager { get; }

        IActiveUnitOfWork CurrentUnitOfWork { get; }

        IRepository<TEntity, TPrimaryKey> EntityRepo { get; }

        IQueryable<TEntity> Query { get; }

        IQueryable<TEntity> QueryAsNoTracking { get; }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindById(TPrimaryKey id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createAndGetId">是否获取id</param>
        /// <returns></returns>
        Task Create(TEntity entity, bool createAndGetId = false);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(TPrimaryKey id);

        /// <summary>
        /// 删除 - 按条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task Delete(List<TPrimaryKey> idList);

    }

    public interface IDomainService<TEntity> : IDomainService<TEntity, long>
         where TEntity : class, IEntity<long>
    {

    }


    public abstract class DomainService<TEntity, TPrimaryKey> : IDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly IServiceProvider _serviceProvider;

        protected DomainService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            UnitOfWorkManager = _serviceProvider.GetRequiredService<IUnitOfWorkManager>();

            EntityRepo = _serviceProvider.GetRequiredService<IRepository<TEntity, TPrimaryKey>>();
        }

        public virtual IUnitOfWorkManager UnitOfWorkManager { get; }

        public virtual IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

        public virtual IRepository<TEntity, TPrimaryKey> EntityRepo { get; protected set; }

        public virtual IQueryable<TEntity> Query => EntityRepo.GetAll();

        public virtual IQueryable<TEntity> QueryAsNoTracking => Query.AsNoTracking();


        public virtual async Task<TEntity> FindById(TPrimaryKey id)
        {
            return await this.EntityRepo.FirstOrDefaultAsync(id);
        }

        public virtual async Task Create(TEntity entity, bool createAndGetId = false)
        {
            switch (createAndGetId)
            {
                case true:
                    await this.EntityRepo.InsertAndGetIdAsync(entity);
                    break;
                case false:
                    await this.EntityRepo.InsertAsync(entity);
                    break;
            }

        }

        public virtual async Task Update(TEntity entity)
        {
            await this.EntityRepo.UpdateAsync(entity);
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            await this.EntityRepo.DeleteAsync(id);
        }

        public virtual async Task Delete(List<TPrimaryKey> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return;
            }

            await this.EntityRepo.DeleteAsync(o => idList.Contains(o.Id));
        }

        public async Task Delete(Expression<Func<TEntity, bool>> predicate)
        {
            await this.EntityRepo.DeleteAsync(predicate);
        }

        public async Task<bool> Exist(TPrimaryKey id)
        {
            var entity = await FindById(id);
            return entity != null;
        }


    }

    public abstract class DomainService<TEntity> : DomainService<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        protected DomainService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

    }
}
