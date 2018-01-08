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
        private const string SwaggerJsonPath = "/swagger/v1/swagger.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
            services.AddMvc();
            services.AddSingleton<IRemotingRefService>(new RemotingRefService());
            services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new Info { Version = "v1", Title = "Fanex.RemotingRef Demo Site", Description = "Demo Site For RemotingRef Client and RemotingRef Service" });
                  c.CustomSchemaIds(type => type.FriendlyId(true));
                  c.DescribeAllEnumsAsStrings();
                  c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Process.GetCurrentProcess().ProcessName}.xml");
              });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc().UseDefaultFiles()
                 .UseStaticFiles()
                 .UseSwagger()
                 .UseSwaggerUI(c => { c.SwaggerEndpoint(SwaggerJsonPath, "IO.Swagger"); });
        }
    }
}