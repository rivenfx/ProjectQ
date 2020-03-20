using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Riven;

namespace Company.Project
{
    public interface ITransientDependency
    {

    }
    public interface ISingletonDependency
    {

    }

    public interface IScopeDependency
    {

    }

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

            services.Scan((scan) =>
            {
                var hostAssembly = typeof(CompanyProjectHostModule).Assembly;
                var hostCoreAssembly = typeof(CompanyProjectHostCoreModule).Assembly;
                var domainAssembly = typeof(CompanyProjectDomainModule).Assembly;
                var efCoreAssembly = typeof(CompanyProjectEntityFrameworkCoreModule).Assembly;
                var applicationAssembly = typeof(CompanyProjectApplicationModule).Assembly;


                scan.FromAssemblies(
                        hostAssembly, 
                        hostCoreAssembly, 
                        domainAssembly, 
                        efCoreAssembly, 
                        applicationAssembly
                    )
                    // Ë²Ê±
                    .AddClasses((classes) =>
                    {
                        classes.AssignableTo<ITransientDependency>();
                    })
                    .AsSelf()
                    .AsMatchingInterface()
                    .WithTransientLifetime()

                     // µ¥Àý
                     .AddClasses((classes) =>
                     {
                         classes.AssignableTo<ISingletonDependency>();
                     })
                     .AsSelf()
                     .AsMatchingInterface()
                     .WithSingletonLifetime()

                      // ·¶Î§
                      .AddClasses((classes) =>
                      {
                          classes.AssignableTo<IScopeDependency>();
                      })
                     .AsSelf()
                     .AsMatchingInterface()
                     .WithScopedLifetime();
            });

            // Riven
            services.AddRivenAspNetCoreModule<CompanyProjectHostModule>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Riven
            app.UseRivenAspNetCoreModule();

        }

    }


}
