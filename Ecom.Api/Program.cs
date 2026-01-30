using AutoMapper;
using Ecom.Api.Controllers.AutoMapper;
using Ecom.infrastructure;
using Microsoft.Extensions.FileProviders;
namespace Ecom.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.InfrastructureConfiguration(builder.Configuration);
            //builder.Services.AddAutoMapper(typeof(CategoryMapping));
            //builder.Services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile<CategoryMapping>();
            //});
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton<IFileProvider>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return env.WebRootFileProvider;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
