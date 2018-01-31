namespace Fanex.RemotingRef.Service.Demo.Ver2
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
            services.AddMvc();
            services.Configure<IConfiguration>(Configuration);
            services.AddSingleton<IRemotingRefService>(new RemotingRefService());
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
                app.UseExceptionHandler(
                   options =>
                   {
                       options.Run(
                         async context =>
                          {
                              context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                              context.Response.ContentType = "text/html";
                              var ex = context.Features.Get<IExceptionHandlerFeature>();

                              if (ex != null)
                              {
                                  var err = $"<h1>Error: {ex.Error.InnerException.Message}</h1>{ex.Error.StackTrace}";
                                  await context.Response.WriteAsync(err).ConfigureAwait(false);
                                  _logger.LogError(ex.Error.Message + ex.Error.StackTrace);
                              }
                          });
                   });
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}