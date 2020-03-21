using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Company.Project.Authenticate.Dtos;
using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riven.Identity.Users;

namespace Company.Project.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : Controller
    {
        readonly IConfiguration _configuration;
        readonly SignInManager _signInManager;
        readonly IHttpClientFactory _httpClientFactory;


        public TokenAuthController(IConfiguration configuration, SignInManager signInManager, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<AuthenticateResultDto> Authenticate([FromBody]AuthenticateModelInput input)
        {
            await Task.Yield();
            var loginResult = await _signInManager.LoginAsync(input.Account, input.Password);

            // 使mvc也登录
            if (loginResult.Result == LoginResultType.Success)
            {
                await this._signInManager.SignInAsync(loginResult.User, false);
            }
            return new AuthenticateResultDto()
            {
                AccessToken = CreateAccessToken(loginResult.Identity.Claims)
            };
        }




        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            DateTime now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
            //issuer: _configuration.Issuer,
            //audience: _configuration.Audience,
            //claims: claims,
            //notBefore: now,
            //expires: now.Add(expiration ?? _configuration.Expiration),
            //signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}