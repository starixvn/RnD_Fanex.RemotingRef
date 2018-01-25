namespace Fanex.RemotingRef.Service.Demo
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Web;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable SA1600 // Elements must be documented

    public class Startup
    {
        private readonly ILogger<Startup> _logger;
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string SwaggerJsonPath = "/swagger/v1/swagger.json";

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
            services.AddMvc();
            services.Configure<IConfiguration>(Configuration);
            services.AddSingleton<IRemotingRefService>(new RemotingRefService());
            //services.AddSwaggerGen(c =>
            //  {
            //      c.SwaggerDoc("v1", new Info { Version = "v1", Title = "Fanex.RemotingRef Demo Site", Description = "Demo Site For RemotingRef Client and RemotingRef Service 2.0" });
            //      c.CustomSchemaIds(type => type.FriendlyId(true));
            //      c.DescribeAllEnumsAsStrings();
            //      c.IncludeXmlComments($"{Directory.GetCurrentDirectory()}/App_Data/xml/Fanex.RemotingRef.Service.Demo.xml");
            //  });
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseExceptionHandler(options =>
            {
                options.Run(
                async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>();

                    if (ex != null)
                    {
                        var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                        await context.Response.WriteAsync(err).ConfigureAwait(false);
                        _logger.LogError(ex.Error.Message);
                    }
                });
            });

            app.UseMvc().UseDefaultFiles()
                 .UseStaticFiles();
            //.UseSwagger()
            //.UseSwaggerUI(c =>
            //       {
            //           c.SwaggerEndpoint(SwaggerJsonPath, "IO.Swagger");
            //       });
        }
    }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore S1075 // URIs should not be hardcoded
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}