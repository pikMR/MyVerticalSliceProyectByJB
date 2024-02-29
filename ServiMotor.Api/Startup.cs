using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using ServiMotor.Infraestructure;
using System;
using ServiMotor.Features.Interfaces;

namespace ServiMotor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var domainAssembly = AppDomain.CurrentDomain.GetAssemblies();
            services.AddMediatR(domainAssembly);
            services.AddAutoMapper(domainAssembly);
            services.Configure<Mongosettings>(Configuration.GetSection("Mongosettings"));
            services.AddControllers();
            services.AddSingleton<IMongoBookDBContext, MongoBookDBContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(i => i.FullName);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiMotor", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiMotor v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
             .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowed(origin => true) // allow any origin
             .AllowCredentials()); // allow credentials
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}