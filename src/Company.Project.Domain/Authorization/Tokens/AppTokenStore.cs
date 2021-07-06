using AspNetCore.Authentication.ApiToken.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiToken;
using Riven.Repositories;
using Riven.Uow;
using Microsoft.EntityFrameworkCore;
using Riven.Extensions;

namespace Company.Project.Authorization.Tokens
{
    public class AppTokenStore : IApiTokenStore
    {
        protected readonly IRepository<AppToken> _tokenRepo;

        public AppTokenStore(IRepository<AppToken> tokenRepo)
        {
            _tokenRepo = tokenRepo;
        }

        [UnitOfWork]
        public virtual async Task<TokenModel> GetAsync(string token, string scheme)
        {
            var appToken = await this._tokenRepo.GetAll()
                 .Where(a => a.Value == token && a.Scheme == scheme)
                 .FirstOrDefaultAsync();

            return appToken;
        }

        [UnitOfWork]
        public virtual async Task<List<TokenModel>> GetListAsync(string userId, string scheme)
        {
            var queryResult = await this._tokenRepo.GetAll()
                .Where(a => a.UserId == userId && a.Scheme == scheme)
                .ToListAsync();

            return queryResult.Select(o => o as TokenModel)
                .ToList();
        }

        [UnitOfWork]
        public virtual async Task<List<TokenModel>> GetListAsync(string userId, string scheme, TokenType type)
        {
            var tokenType = type.ToString();

            var queryResult = await this._tokenRepo.GetAll()
                .Where(a => a.UserId == userId
                    && a.Scheme == scheme
                    && a.Type == type)
                .ToListAsync();

            return queryResult.Select(o => o as TokenModel)
                .ToList();
        }

        [UnitOfWork]
        public virtual async Task RemoveAsync(string token, string scheme)
        {
            var tokenEntity = await this._tokenRepo.GetAll()
                .Where(a => a.Value == token)
                .FirstOrDefaultAsync();
            if (tokenEntity != null)
            {
                await this._tokenRepo.DeleteAsync(tokenEntity);
            }
        }

        [UnitOfWork]
        public virtual async Task<int> RemoveExpirationAsync()
        {
            var tokens = this._tokenRepo.GetAll().Where(a => a.Expiration < DateTime.Now);
            var count = await tokens.CountAsync();
            await this._tokenRepo.DeleteAsync(tokens);
            return count;
        }

        [UnitOfWork]
        public virtual async Task RemoveListAsync(string userId, string scheme)
        {
            var tokenList = await this._tokenRepo.GetAll()
              .Where(a => a.UserId == userId
                   && a.Scheme == scheme)
              .ToListAsync();

            if (tokenList.Any())
            {
                await this._tokenRepo.DeleteAsync(tokenList);
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveListAsync(string userId, string scheme, TokenType type)
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
        }

        [UnitOfWork]
        public virtual async Task StoreAsync(TokenModel token)
        {
            await this._tokenRepo.InsertAsync(token.MapTo<AppToken>());
        }

        [UnitOfWork]
        public virtual async Task StoreAsync(List<TokenModel> token)
        {
            await this._tokenRepo.InsertAsync(token.MapTo<List<AppToken>>());
        }

        [UnitOfWork]
        public virtual async Task UpdateAsync(TokenModel token)
        {
            await this._tokenRepo.UpdateAsync(token.MapTo<AppToken>());
        }

        [UnitOfWork]
        public virtual async Task UpdateListAsync(List<TokenModel> token)
        {
            await this._tokenRepo.UpdateAsync(token.MapTo<List<AppToken>>());
        }
    }
}
