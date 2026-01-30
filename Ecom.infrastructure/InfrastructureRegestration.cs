using Ecom.core.Interfaces;
using Ecom.infrastructure.Repositires;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ecom.core.Services;
using Ecom.infrastructure.Repositires.Service;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
namespace Ecom.infrastructure
{
    public static class InfrastructureRegestration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagementServicecs, ImageManagementServicecs>();

            services.AddSingleton<IConnectionMultiplexer>
                (i=>
                 {
                     //var configurationOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                     //return ConnectionMultiplexer.Connect(configurationOptions);
                     var options = new ConfigurationOptions
                     {
                         EndPoints = { "localhost" },
                         AbortOnConnectFail = false
                     };
                     return ConnectionMultiplexer.Connect(options);

                 }
                );
            
            services.AddDbContext<AppDbContext>(op => {
                op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            });
            return services;
        }

    }
}
