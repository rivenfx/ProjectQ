using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Company.Project.Authenticate.Dtos;
using Company.Project.Authorization;
using Company.Project.Authorization.Users;
using Company.Project.Configuration;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Riven.Exceptions;
using Riven.Authorization;
using Riven.Identity.Users;
using Riven.Security;

namespace Company.Project.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : Controller
    {
        static TimeSpan expiration = new TimeSpan(0, 30, 0);

        readonly IConfiguration _configuration;
        readonly SignInManager _signInManager;
        readonly IHttpClientFactory _httpClientFactory;


        public TokenAuthController(IConfiguration configuration, SignInManager signInManager, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AuthenticateResultDto> Authenticate([FromBody] AuthenticateModelInput input)
        {
            var loginResult = await _signInManager.LoginAsync(input.Account, input.Password);


            if (loginResult.Result != LoginResultType.Success)
            {
                throw new UserFriendlyException("登录失败! 用户名或密码错误");
            }

            var result = new AuthenticateResultDto();

            // 使mvc也登录
            if (input.UseCookie)
            {
                await this._signInManager.SignInWithClaimsIdentityAsync(loginResult.ClaimsPrincipal, input.RememberClient);
            }
            if (input.UseToken)
            {
                result.AccessToken = CreateAccessToken(loginResult.Identity.Claims, expiration);
                result.EncryptedAccessToken = SimpleStringCipher.Instance.Encrypt(result.AccessToken);
                result.ExpireInSeconds = expiration.TotalSeconds;
            }

            return result;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize]
        [HttpPost]
        public async Task<AuthenticateResultDto> RefreshToken()
        {
            var result = new AuthenticateResultDto();

            // TODO
            var userId = "";

            var loginResult = await _signInManager.LoginByUserIdAsync(userId);

            result.AccessToken = CreateAccessToken(loginResult.Identity.Claims, expiration);
            result.ExpireInSeconds = expiration.TotalSeconds;

            return result;
        }

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="claims">携带信息</param>
        /// <param name="expiration">过期时间</param>
        /// <returns></returns>
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan expiration)
        {
            /*
                iss: jwt签发者
                sub: jwt所面向的用户
                aud: 接收jwt的一方
                exp: jwt的过期时间，这个过期时间必须要大于签发时间
                nbf: 定义在什么时间之前，该jwt都是不可用的
                iat: jwt的签发时间
                jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击
             */

            var claimsList = claims.ToList();

            //var defaultNameIdentifier = claimsList.First(o => o.Type == ClaimTypes.NameIdentifier);

            //claimsList.Add(new Claim(JwtRegisteredClaimNames.Sub, defaultNameIdentifier.Value));
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));


            var now = DateTime.UtcNow;
            var jwtBearerInfo = _configuration.GetJwtBearerInfo();

            var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtBearerInfo.SecurityKey)),
                    SecurityAlgorithms.HmacSha256
                );

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwtBearerInfo.Issuer,       // 发行方
                    audience: jwtBearerInfo.Audience,   // 接收方
                    claims: claimsList,                 // 
                    notBefore: now,
                    expires: now.Add(expiration),
                    signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}