using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using Newtonsoft.Json.Converters;

using Riven;
using Riven.AspNetCore.Accessors;
using Riven.Modular;
using Riven.Uow;
using Riven.MultiTenancy;

using Company.Project.Authorization;
using Company.Project.Configuration;
using Company.Project.Debugger;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Configuration;

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectHostCoreModule),
        typeof(RivenListViewInfoAspNetCoreHostModule),
        typeof(EasyCachingModule)
        )]
    public class CompanyProjectHostModule : AppModule
    {
        static string CorsPolicyName = "RivenDefaultCorsPolicy";

        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectHostModule>();

            // 添加获取当前连接字符串提供者
            context.Services
                .AddRivenCurrentConnectionStringNameProvider<AspNetCoreCurrentConnectionStringNameProvider>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;


            #region AspNetCore - Mvc

            // aspnet core mvc
            var mvcBuilder = context.Services.AddMvc();

            // mvc options
            mvcBuilder.AddNewtonsoftJson((options) =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            // razor pages options
            mvcBuilder.AddRazorPagesOptions((options) =>
                 {

                 });

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
                    var corsOrigins = configuration.GetAppInfo().CorsOrigins
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

            // 注册 asp.net core identity
            context.Services.IdentityRegister(configuration);
            // 配置校验
            context.Services.AuthenticationConfiguration(configuration);

            #endregion



            #region 配置AOP

            context.Services.ConfigureDynamicProxy(config =>
               {
                   // UnitOfWork 拦截器
                   config.AddRivenUnitOfWork();
               });

            #endregion
        }

        public override void OnPostConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;

            var appInfo = configuration.GetAppInfo();

            #region Riven - AspNetCore 请求本地化

            context.Services.AddRivenRequestLocalization();

            #endregion


            #region Riven - AspNetCore 服务注册和配置

            // Riven - Swagger 和 动态WebApi
            context.Services.AddRivenAspNetCoreSwashbuckle(
                (options) =>
                {
                    var apiInfo = new OpenApiInfo()
                    {
                        Title = appInfo.Name,
                        Version = appInfo.Version
                    };
                    options.SwaggerDoc(apiInfo.Version, apiInfo);
                    options.DocumentFilter<BaseHrefDocumentFilter>(appInfo.Basehref);
                },
                (options) =>
                {
                    // 注册指定程序集对应的 url 和 http 请求方式
                    options.AddAssemblyOptions(typeof(CompanyProjectApplicationModule).Assembly, options.DefaultApiPrefix, "POST");

                    // 添加 listview 模块
                    options.UseRivenListViewInfo();
                });

            context.Services.AddSwaggerGenNewtonsoftSupport();

            // Riven - AspNetCore 基础服务与配置
            context.Services.AddRivenAspNetCore((options) =>
            {
                if (DebugHelper.IsDebug)
                {
                    // 发送所有异常数据到客户端
                    options.SendAllExceptionToClient = true;
                }
            });

            // Riven - AspNetCore 过滤器
            context.Services.AddRivenAspNetCoreFilters();

            // Riven - AspNetCore Uow实现
            context.Services.AddRivenAspNetCoreUow();

            #endregion
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var configuration = context.Configuration;
            var app = context.ServiceProvider.GetService<IApplicationBuilderAccessor>().ApplicationBuilder;
            var env = context.ServiceProvider.GetService<IWebHostEnvironment>();

            var appInfo = configuration.GetAppInfo();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            #region AspNetCore - UseStaticFiles / UseRouting /UseCors


            app.UseStaticFilesWithBaseHref(appInfo.Basehref);

            app.UseRouting();
            app.UseCors(CorsPolicyName);

            #endregion



            #region Riven - AspNetCore  ExceptionHandling / Uow / RequestLocalization

            // ExceptionHandling
            app.UseRivenAspNetCoreExceptionHandling();

            // Uow
            app.UseRivenAspnetCoreUow();

            // RequestLocalization
            app.UseRivenRequestLocalization();

            #endregion



            #region App - AspNetCore Auth

            // 认证配置
            app.UseAppAuthenticationAndAuthorization();

            #endregion



            #region Riven -启用并配置 Swagger 和 SwaggerUI

            //  Riven - Swagger
            app.UseRivenAspNetCoreSwashbuckle((swaggerUiOption) =>
            {
                swaggerUiOption.SwaggerEndpoint(
                       $"{appInfo.Basehref}/swagger/{appInfo.Version}/swagger.json",
                       appInfo.Name
                   );
                swaggerUiOption.EnableDeepLinking();
                swaggerUiOption.DocExpansion(DocExpansion.None);

                // 应用公共的js
                swaggerUiOption.InjectJavascript($"{appInfo.Basehref}/views/app.js");

                // swagger 定制的样式和脚本
                swaggerUiOption.InjectStylesheet($"{appInfo.Basehref}/views/swagger/index.css");
                swaggerUiOption.InjectJavascript($"{appInfo.Basehref}/views/swagger/index.js");

            });

            #endregion



            #region AspNetCore - Endpoints

            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapDefaultControllerRoute();

                   endpoints.MapRazorPages();

                   // 自定义路由

               });

            #endregion

        }
    }
}
