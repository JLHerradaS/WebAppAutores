﻿using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebAppAutores.Filtros;
using WebAppAutores.Middlewares;

namespace WebAppAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // dependency injection
        // Services configuration  
        // authom resolves all the classes dependencies
        // what we define here gets instanciated properly with all config when used in a class
        public void ConfigureServices(IServiceCollection services)
        {
            // adding filter
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(MyExceptionFilter));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).
                AddNewtonsoftJson(); // for the HTTP patch that uses JsonPatchDocument

            // when ApplicationDbContext is a dep, it gets instanciated properly with all configs
            // Scope service by default 
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

             // Microsoft.AspNetCore.Authentication.JwtBearer package needed for authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(Startup));
        }

        // this method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            // MIDDLEWARES

            // calling same custom middleware but using static extension class
            app.UseLogResponseHTTP();  

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
