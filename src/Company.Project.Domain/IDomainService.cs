using Microsoft.EntityFrameworkCore;

using Riven.Dependency;
using Riven.Entities;
using Riven.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Project
{
    public interface IDomainService<TEntity, TPrimaryKey> : ITransientDependency
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IRepository<TEntity, TPrimaryKey> EntityRepo { get; }

        IQueryable<TEntity> Query { get; }

        IQueryable<TEntity> QuerAsNoTracing { get; }

    }

    public interface IDomainService<TEntity> : IDomainService<TEntity, long>
         where TEntity : class, IEntity<long>
    {

    }


    public abstract class DomainService<TEntity, TPrimaryKey> : IDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected DomainService(IRepository<TEntity, TPrimaryKey> entityRepo)
        {
            EntityRepo = entityRepo;
        }

        public virtual IRepository<TEntity, TPrimaryKey> EntityRepo { get; protected set; }

        public virtual IQueryable<TEntity> Query => EntityRepo.GetAll();

        public virtual IQueryable<TEntity> QuerAsNoTracing => Query.AsNoTracking();

    }

    public abstract class DomainService<TEntity> : DomainService<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        protected DomainService(IRepository<TEntity, long> entityRepo)
            : base(entityRepo)
        {

        }

    }
}
