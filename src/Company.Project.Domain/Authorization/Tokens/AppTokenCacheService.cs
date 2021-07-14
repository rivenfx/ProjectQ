using AspNetCore.Authentication.ApiToken;
using AspNetCore.Authentication.ApiToken.Abstractions;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Riven.MultiTenancy;

namespace Company.Project.Authorization.Tokens
{
    public class AppTokenCacheService : IApiTokenCacheService
    {
        readonly ConcurrentDictionary<string, TokenModelCache> _dict;
        readonly IMultiTenancyProvider _tenancyProvider;

        public AppTokenCacheService()
        {
            _dict = new ConcurrentDictionary<string, TokenModelCache>();
        }

        public AppTokenCacheService(IMultiTenancyProvider tenancyProvider)
        {
            _tenancyProvider = tenancyProvider;
        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task<TokenModelCache> GetAsync(string token, string scheme)
        {
            await Task.Yield();

            var key = GetKey(scheme, token);

            if (this._dict.TryGetValue(key, out var res))
            {
                return res;
            }

            return null;
        }

        public virtual async Task LockReleaseAsync(string key, string value)
        {
            await Task.CompletedTask;
        }

        public virtual async Task<bool> LockTakeAsync(string key, string value, TimeSpan timeOut)
        {
            await Task.Yield();


            return true;

        }

        public virtual async Task RemoveAsync(string token, string scheme)
        {
            var key = GetKey(scheme, token);

            this._dict.TryRemove(key, out var val);

            await Task.CompletedTask;
        }

        public virtual async Task SetAsync(TokenModel token)
        {
            var key = GetKey(token.Scheme, token.Value);

            var cache = new TokenModelCache() { Token = token };

            this._dict.AddOrUpdate(key, cache, (ok, ov) =>
            {
                return cache;
            });

            await Task.CompletedTask;
        }

        public virtual async Task SetNullAsync(string invalidToken, string scheme)
        {
            var key = GetKey(scheme, invalidToken);

            var cache = new TokenModelCache();

            this._dict.AddOrUpdate(key, cache, (ok, ov) =>
            {
                return cache;
            });

            await Task.CompletedTask;
        }

        protected virtual string GetKey(string token, string scheme)
        {
            var tenant = _tenancyProvider.CurrentTenantNameOrNull() ?? string.Empty;

            return string.Format("scheme:{0}-{1}:token:{2}", scheme, tenant, token);
        }
    }
}
