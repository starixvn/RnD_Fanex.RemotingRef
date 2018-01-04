namespace Fanex.RemotingRef.Service.Demo
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddMvc();
            services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new Info { Version = "v1", Title = "Fanex.RemotingRef.Service Supported Functions", Description = "anything" });
                  c.CustomSchemaIds(type => type.FriendlyId(true));
                  c.DescribeAllEnumsAsStrings();
                  c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Process.GetCurrentProcess().ProcessName}.xml");
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc().UseDefaultFiles()
                 .UseStaticFiles()
                 .UseSwagger()
                 .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "IO.Swagger"); });
        }
    }
}