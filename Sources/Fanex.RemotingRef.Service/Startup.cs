namespace Fanex.RemotingRef.Service
{
    using global::RemotingRef;
    using MessagePack.AspNetCoreMvcFormatter;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RefNewData;

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
            services.AddSingleton<IRefData>(new RefNewDataHandler());
            services.AddMvc(option =>
            {
                option.OutputFormatters.Add(new MessagePackOutputFormatter());
                option.InputFormatters.Add(new MessagePackInputFormatter(MessagePack.Resolvers.ContractlessStandardResolver.Instance));
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