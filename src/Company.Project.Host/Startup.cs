using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Riven;

namespace Company.Project
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Riven
            services.AddRivenAspNetCoreModule<CompanyProjectHostModule>(Configuration);


            services.ConfigureDynamicProxy((configuration) =>
            {
                configuration.Interceptors.AddTyped<RivenUnitOfWorkInterceptor>(method =>
                {
                    if (method.DeclaringType.BaseType != null)
                    {

                    }
                    if (method.Name.Contains("HandleAuthenticate"))
                    {
                        return true;
                    }

                    return false;
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Riven
            app.UseRivenAspNetCoreModule();

        }

    }


}
