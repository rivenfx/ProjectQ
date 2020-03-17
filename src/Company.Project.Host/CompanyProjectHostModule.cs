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

namespace Company.Project
{
    [DependsOn(
        typeof(CompanyProjectHostCoreModule)
        )]
    public class CompanyProjectHostModule : AppModule
    {
        static string CorsPolicyName = "RivenDefaultCorsPolicy";

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;

            // aspnet core mvc
            context.Services.AddControllersWithViews();
            context.Services.AddHttpContextAccessor();


            // riven 相关模块初始化
            context.Services.AddRivenAspNetCoreSwashbuckle((options) =>
            {
                var apiInfo = new OpenApiInfo()
                {
                    Title = configuration["App:Name"],
                    Version = configuration["App:Version"]
                };
                options.SwaggerDoc(apiInfo.Version, apiInfo);
                options.DocInclusionPredicate((docName, description) => true);
            });
            context.Services.AddRivenAspNetCoreUow((uowAttr) =>
            {

            });
            context.Services.AddRivenAspNetCore();

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


            // 添加默认的UnitOfWork数据
            context.Services.AddSingleton<UnitOfWorkAttribute>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var configuration = context.Configuration;

            var app = context.ServiceProvider.GetService<IApplicationBuilderAccessor>().ApplicationBuilder;

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CorsPolicyName);

            app.UseRivenAspNetCoreSwashbuckle((swaggerUiOption) =>
            {
                swaggerUiOption.SwaggerEndpoint(
                       $"/swagger/{configuration["App:Version"]}/swagger.json",
                       configuration["App:Name"]
                   );
            }, true);

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
        }
    }
}
