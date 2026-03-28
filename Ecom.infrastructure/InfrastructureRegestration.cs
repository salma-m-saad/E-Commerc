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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Ecom.core.Entities;
using Microsoft.AspNetCore.Identity;
namespace Ecom.infrastructure
{
    public static class InfrastructureRegestration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

            
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IImageManagementServicecs, ImageManagementServicecs>();
            services.AddScoped<IGenerateToken, GenerateTokenService>();

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

            services.AddScoped<IAuth, AuthRepositry>();

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAuthentication(op => 
                {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }
             ).AddCookie(C=>
               {
                   C.Cookie.Name = "token";
                   C.Events.OnRedirectToLogin = context =>
                   {
                       context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                       return Task.CompletedTask;
                   };

               }
             ).AddJwtBearer(J => 
             {
                 J.RequireHttpsMetadata = false;
                 J.SaveToken = true;
                 J.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
                     ValidateIssuer = true,
                     ValidIssuer = configuration["Token:Issure"],
                     ValidateAudience = false,
                     ClockSkew = TimeSpan.Zero,
                 };
                 J.Events = new JwtBearerEvents()
                 {
                     OnMessageReceived = context =>
                     {
                         context.Token = context.Request.Cookies["token"];
                         return Task.CompletedTask;
                     }
                 };
             }
             );
            return services;
        }

    }
}
