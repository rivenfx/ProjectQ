using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Riven;
using Riven.AspNetCore.Accessors;
using Riven.AspNetCore.Mvc.Uow;
using Riven.Modular;
using Riven.Uow;
using Company.Project.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Riven.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectHostCoreModule)
        )]
    public class CompanyProjectHostModule : AppModule
    {
        static string CorsPolicyName = "RivenDefaultCorsPolicy";

        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectHostModule>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;


            #region AspNetCore - Mvc

            // aspnet core mvc
            var mvcBuilder = context.Services.AddControllersWithViews();
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif

            #endregion


            #region AspNetCore - HttpContextAccessor / HttpClient
            
            context.Services.AddHttpContextAccessor();
            context.Services.AddHttpClient();

            #endregion


            #region AspNetCore - Cors
            
            // aspnet core 跨域
            context.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    // 配置跨域
                    var corsOrigins = configuration["App:CorsOrigins"]
                                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                        .Select(o => o.TrimEnd('/'))
                                        .Where(o => o != "*")
                                        .ToArray();

                    builder
                        .WithOrigins(
                            corsOrigins
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            #endregion


            #region AspNetCore - Identity And Auth
            
            // 认证配置
            context.Services.IdentityRegister();
            context.Services.IdentityConfiguration(configuration);

            #endregion


            #region Riven - AspNetCore 请求本地化

            context.Services.AddRivenRequestLocalization();

            #endregion


            #region Riven - AspNetCore 服务注册和配置

            // Riven - Swagger 和 动态WebApi
            context.Services.AddRivenAspNetCoreSwashbuckle((options) =>
            {
                var apiInfo = new OpenApiInfo()
                {
                    Title = configuration[AppConsts.AppNameKey],
                    Version = configuration[AppConsts.AppVersionKey]
                };
                options.SwaggerDoc(apiInfo.Version, apiInfo);
            });
            
            // Riven - AspNetCore Uow实现
            context.Services.AddRivenAspNetCoreUow();
            
            // Riven - AspNetCore 基础服务相关
            context.Services.AddRivenAspNetCore((options) =>
            {
                // 启用Uow
                options.UnitOfWorkFilterEnable = true;
            });

            #endregion


        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var configuration = context.Configuration;
            var app = context.ServiceProvider.GetService<IApplicationBuilderAccessor>().ApplicationBuilder;
            var env = context.ServiceProvider.GetService<IWebHostEnvironment>();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region AspNetCore - UseStaticFiles / UseRouting /UseCors
            
            
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CorsPolicyName);

            #endregion


            #region App - AspNetCore Auth
            
            // 认证配置
            app.UseAppAuthenticationAndAuthorization();

            #endregion


            #region Riven - AspNetCore 请求本地化

            app.UseRivenRequestLocalization();

            #endregion


            #region AspNetCore - Endpoints
            
            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapControllerRoute(
                           "defaultWithArea",
                           "{area}/{controller=Home}/{action=Index}/{id?}"
                       );
                   endpoints.MapControllerRoute(
                           "default",
                           "{controller=Home}/{action=Index}/{id?}"
                       );

                // 自定义路由

            }); 

            #endregion


            #region Riven -启用并配置 Swagger 和 SwaggerUI

            //  Riven - Swagger
            app.UseRivenAspNetCoreSwashbuckle((swaggerUiOption) =>
            {
                swaggerUiOption.SwaggerEndpoint(
                       $"/swagger/{configuration[AppConsts.AppVersionKey]}/swagger.json",
                       configuration[AppConsts.AppNameKey]
                   );
                swaggerUiOption.EnableDeepLinking();
                swaggerUiOption.DocExpansion(DocExpansion.None);
            });

            #endregion

        }
    }
}
