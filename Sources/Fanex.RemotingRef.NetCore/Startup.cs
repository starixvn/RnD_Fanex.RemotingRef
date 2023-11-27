using MessagePack.Resolvers;
using MessagePack.AspNetCoreMvcFormatter;

namespace Fanex.RemotingRef.NetCore;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
        .AddFanexLogging(Configuration)
        .AddApplicationServices()
        .AddSwaggerGen()
        .AddControllers();

        services.AddHealthChecks();
        services.AddMvc().AddMvcOptions(option =>
        {
            option.OutputFormatters.Clear();
            option.OutputFormatters.Add(new MessagePackOutputFormatter(MessagePack.MessagePackSerializerOptions.Standard));
            option.InputFormatters.Clear();
            option.InputFormatters.Add(new MessagePackInputFormatter(MessagePack.MessagePackSerializerOptions.Standard));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpLogging();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.AddRefDataClient(Configuration);
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseEndpoints(endpoints =>
       {
           endpoints.MapDefaultControllerRoute();
           endpoints.MapHealthChecks("/health").AllowAnonymous();
       });
    }
}