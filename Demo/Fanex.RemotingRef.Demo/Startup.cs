namespace Fanex.RemotingRef.Demo
{
    using System;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using global::RemotingRef;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RefNewData;

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
            RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
            services.AddMvc();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<IRefData, RefNewDataHandler>();
            services.AddSingleton<IRemotingRefDataAccess, RemotingRefDataAccess>();
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
}