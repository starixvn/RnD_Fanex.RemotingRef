namespace Fanex.RemotingRef.Service
{
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using MessagePack.AspNetCoreMvcFormatter;
    using MessagePack.Resolvers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRefDataHelper>(new RefDataHelper());
            services.AddMvc(option =>
            {
                option.OutputFormatters.Add(new MessagePackOutputFormatter());
                option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Instance));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }
}