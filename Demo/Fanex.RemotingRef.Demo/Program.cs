using Fanex.RemotingRef;
using RefClient.Factory;
using RefDataModelStandard.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer()
    .AddControllersWithViews();

builder.Services.AddHealthChecks();

RefDataTypeContainer
    .ConnectTo(new Uri(configuration["RemotingRefService"]))
    .FromApplication("Fanex.RemotingRef.Demo");
RefClientFactory.InitializeConfig(new ClientConfigModel
{
    RefDataWebApiUrl = new RefDataWebApiUrl
    {
        Url = configuration["TW.RemotingRef.API"]
    },
    RefDataLocalRedisConfiguration = new RedisConfiguration
    {
        Host = configuration["TW.RemotingRef.Redis"].Split(',').ToList(),
        DatabaseId = int.Parse(configuration["TW.RemotingRef.DatabaseId"]),
    }
});

builder.Services.AddSingleton<IRemotingRefService, RemotingRefService>();
builder.WebHost.UseKestrel();
var app = builder.Build();

// Thêm Exception Handling Middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (exception != null)
        {
            var error = new
            {
                Message = "An error occurred while processing your request.",
                ExceptionType = exception.Error.GetType().Name,
                Details = exception.Error.Message
            };
            
            await context.Response.WriteAsJsonAsync(error);
        }
    });
});

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=BetTypeName}/{action=BetTypeName}/{id?}");
    
    endpoints.MapHealthChecks("/health").AllowAnonymous();
});

app.Run();