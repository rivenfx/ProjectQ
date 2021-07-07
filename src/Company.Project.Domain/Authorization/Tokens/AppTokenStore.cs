using AspNetCore.Authentication.ApiToken.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiToken;
using Riven.Repositories;
using Riven.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riven.Extensions;
using Company.Project.MultiTenancy;

namespace Company.Project.Authorization.Tokens
{
    public class AppTokenStore : IApiTokenStore
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IUnitOfWorkManager _uowManager;
        protected readonly IRepository<AppToken> _tokenRepo;

        public AppTokenStore(IServiceProvider serviceProvider, IUnitOfWorkManager uowManager, IRepository<AppToken> tokenRepo)
        {
            _serviceProvider = serviceProvider;
            _uowManager = uowManager;
            _tokenRepo = tokenRepo;
        }

        public virtual async Task<TokenModel> GetAsync(string token, string scheme)
        {
            return await this.UowWork(async () =>
            {
                var appToken = await this._tokenRepo.GetAll()
                 .Where(a => a.Value == token && a.Scheme == scheme)
                 .FirstOrDefaultAsync();

                return appToken;
            });

        }


        public virtual async Task<List<TokenModel>> GetListAsync(string userId, string scheme)
        {
            return await this.UowWork(async () =>
            {
                var queryResult = await this._tokenRepo.GetAll()
                .Where(a => a.UserId == userId && a.Scheme == scheme)
                .ToListAsync();

                return queryResult.Select(o => o as TokenModel)
                    .ToList();

            });

        }


        public virtual async Task<List<TokenModel>> GetListAsync(string userId, string scheme, TokenType type)
        {
            return await this.UowWork(async () =>
             {
                 var tokenType = type.ToString();

                 var queryResult = await this._tokenRepo.GetAll()
                     .Where(a => a.UserId == userId
                         && a.Scheme == scheme
                         && a.Type == type)
                     .ToListAsync();

                 return queryResult.Select(o => o as TokenModel)
                     .ToList();
             });

        }

        public virtual async Task RemoveAsync(string token, string scheme)
        {
            await this.UowWork(async () =>
            {
                var tokenEntity = await this._tokenRepo.GetAll()
               .Where(a => a.Value == token)
               .FirstOrDefaultAsync();
                if (tokenEntity != null)
                {
                    await this._tokenRepo.DeleteAsync(tokenEntity);
                }
            });

        }

        public virtual async Task<int> RemoveExpirationAsync()
        {
            return await this.UowWork(async () =>
             {
                 var resultCount = 0;

                 // 获取当前工作单元
                 var currentUnitOfWork = this._uowManager.Current;

                 // 获取所有租户
                 var repo = _serviceProvider.GetService<IRepository<Tenant, Guid>>();
                 var tenants = await repo.GetAll().Select(o => o.Name).ToListAsync();

                 var deleteFunc = new Func<Task<int>>(async () =>
                 {
                     var tokens = this._tokenRepo.GetAll().Where(a => a.Expiration < DateTime.UtcNow);
                     var count = await tokens.CountAsync();
                     await this._tokenRepo.DeleteAsync(tokens);

                     return count;
                 });

                 // 宿主删除
                 resultCount += await deleteFunc();

                 // 租户删除
                 foreach (var tenant in tenants)
                 {
                     using (currentUnitOfWork.ChangeTenant(tenant))
                     {
                         resultCount += await deleteFunc();
                     }
                 }

                 return resultCount;
             });

        }


        public virtual async Task RemoveListAsync(string userId, string scheme)
        {
            await this.UowWork(async () =>
            {
                var tokenList = await this._tokenRepo.GetAll()
              .Where(a => a.UserId == userId
                   && a.Scheme == scheme)
              .ToListAsync();

                if (tokenList.Any())
                {
                    await this._tokenRepo.DeleteAsync(tokenList);
                }
            });
        }

        public virtual async Task RemoveListAsync(string userId, string scheme, TokenType type)
        {
            await this.UowWork(async () =>
            {
                var tokenList = await this._tokenRepo.GetAll()
               .Where(a => a.UserId == userId
                    && a.Scheme == scheme
                    && a.Type == type)
               .ToListAsync();

                if (tokenList.Any())
                {
                    await this._tokenRepo.DeleteAsync(tokenList);
                }
            });


        }

        public virtual async Task StoreAsync(TokenModel token)
        {
            await this.UowWork(async () =>
            {
                var entity = token.MapTo<AppToken>();
                entity.CreateTime = entity.CreateTime.ToUniversalTime();
                entity.Expiration = entity.Expiration.ToUniversalTime();
                await this._tokenRepo.InsertAsync(entity);
            });

        }


        public virtual async Task StoreAsync(List<TokenModel> token)
        {
            await this.UowWork(async () =>
            {
                var entitys = token.MapTo<List<AppToken>>();
                entitys.ForEach(o =>
                {
                    o.CreateTime = o.CreateTime.ToUniversalTime();
                    o.Expiration = o.Expiration.ToUniversalTime();
                });

                await this._tokenRepo.InsertAsync(entitys);
            });
        }


        public virtual async Task UpdateAsync(TokenModel token)
        {
            await this.UowWork(async () =>
            {

                var entity = token.MapTo<AppToken>();
                entity.CreateTime = entity.CreateTime.ToUniversalTime();
                entity.Expiration = entity.Expiration.ToUniversalTime();

                await this._tokenRepo.UpdateAsync(entity);
            });
        }

        public virtual async Task UpdateListAsync(List<TokenModel> token)
        {
            await this.UowWork(async () =>
            {
                var entitys = token.MapTo<List<AppToken>>();
                entitys.ForEach(o =>
                {
                    o.CreateTime = o.CreateTime.ToUniversalTime();
                    o.Expiration = o.Expiration.ToUniversalTime();
                });

                await this._tokenRepo.UpdateAsync(entitys);
            });
        }



        protected virtual async Task<TResult> UowWork<TResult>(Func<Task<TResult>> func)
        {
            TResult res = default;
            using (var uow = this._uowManager.Begin())
            {
                res = await func.Invoke();
                await uow.CompleteAsync();
            }

            return res;
        }

        protected virtual async Task UowWork(Action action)
        {
            using (var uow = this._uowManager.Begin())
            {
                action.Invoke();
                await uow.CompleteAsync();
            }
        }
    }
}
