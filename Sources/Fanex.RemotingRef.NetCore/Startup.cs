using Microsoft.AspNetCore.Diagnostics;
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
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();
        services.AddFanexLogging(Configuration);
        services.AddMvc().AddMvcOptions(option =>
        {
            option.OutputFormatters.Clear();
            option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
            option.InputFormatters.Clear();
            option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();

        }

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>();
                if (ex == null)
                {
                    await context.Response.WriteAsync(ex.Error.Message);
                    return;
                }



                throw ex.Error;
            });
        });

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