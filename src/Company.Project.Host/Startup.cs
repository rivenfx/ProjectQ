using Company.Project.Wrappers;
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
            services.AddSingleton<ApplicationBuilderWrapper>();

            // Riven
            services.AddRivenModule<CompanyProjectHostModule>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 
            app.ApplicationServices.GetService<ApplicationBuilderWrapper>().ApplicationBuilder = app;

            // Riven
            app.ApplicationServices.UseRivenModule();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

    }

  
}
