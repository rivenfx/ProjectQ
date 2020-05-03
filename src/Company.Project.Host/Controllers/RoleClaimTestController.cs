using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Company.Project.Authenticate.Dtos;
using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riven.Identity.Authorization;
using Riven.Identity.Users;

namespace Company.Project.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RoleClaimTestController : Controller
    {
        readonly IConfiguration _configuration;
        readonly SignInManager _signInManager;
        readonly IHttpClientFactory _httpClientFactory;


        public RoleClaimTestController(IConfiguration configuration, SignInManager signInManager, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [ClaimsAuthorize("HelloWord")]
        public async Task<string> Authenticate(string input)
        {
            await Task.Yield();

            return input;
        }


    }
}