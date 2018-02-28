namespace Fanex.RemotingRef.Demo
{
    using System;
    using System.Net;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using global::RemotingRef;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RefNewData;

    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
            services.AddMvc();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<IRefData, RefNewDataHandler>();
            services.AddSingleton<IRemotingRefDataAccess, RemotingRefDataAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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
                                _logger.LogError(ex.Error.GetBaseException().Message + ex.Error.StackTrace);
                            }
                        });
                    });
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}