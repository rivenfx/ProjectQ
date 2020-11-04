using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using Company.Project.Configuration;
using System.Linq;

namespace Company.Project.Database
{
    public abstract class DesignTimeDbContextFactoryBase<TContext>
        : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public virtual TContext CreateDbContext(string[] args)
        {
            var dbContextOptionsType = typeof(DbContextOptions);
            var dbContextType = typeof(TContext);


            var constructors = dbContextType.GetConstructors();
            var constructor = constructors.Where(o =>
             {
                 var parameterInfos = o.GetParameters();

                 var dbContextOptionsParameter = parameterInfos
                           .FirstOrDefault(parameter => parameter.ParameterType.GetInterface("IDbContextOptions") != null);

                 return dbContextOptionsParameter != null;

             }).FirstOrDefault();

            if (constructor == null)
            {
                throw new ArgumentException($"Riven: The DbContextType {dbContextType.FullName} does not have a constructor with a DbContextOptions argument");
            }

            var dbContextOptions = this.GetDbContextOptions();

            var obj = constructor.Invoke(new object[] {
                dbContextOptions,
                null
            });

            return obj as TContext;
        }


        protected virtual DbContextOptions GetDbContextOptions()
        {
            var configuration = this.BuildConfiguration();

            var connectionString = configuration.GetDefaultDatabaseConnectionString();

            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} start");
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} current database connection string: {connectionString}");

            var builder = new DbContextOptionsBuilder();

            AppDbContextConfigurer.Configure(builder, configuration, connectionString);


            return builder.Options;
        }

        protected virtual IConfiguration BuildConfiguration()
        {
            return ConfigurationHelper.GetConfiguration("appsettings");
        }
    }
}
