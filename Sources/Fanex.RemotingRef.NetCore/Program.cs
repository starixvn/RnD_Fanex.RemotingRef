
using Fanex.RemotingRef.NetCore;
using Fanex.RemotingRef.NetCore.Services;
using MessagePack.AspNetCoreMvcFormatter;
using System.Configuration;

//var builder = CreateHostBuilder(args);

//var host = builder.Build();
//host.Run();

//static IHostBuilder CreateHostBuilder(string[] args) =>
//    Host.CreateDefaultBuilder(args)
//        .ConfigureWebHostDefaults(webBuilder =>
//        {
//            webBuilder.UseStartup<Startup>();
//            if (!app.Environment.IsDevelopment())
//            {
//                builder.WebHost.UseUrls("http://0.0.0.0:5000");
//            }
//            webBuilder.UseUrls("http://0.0.0.0:5000");
//        });

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
       .Configure<AppSettings>(builder.Configuration)
       .AddFanexLogging(builder.Configuration)
       .AddApplicationServices()
       .AddSwaggerGen()
       .AddControllers();

builder.Services.AddHealthChecks();
builder.Services.AddMvc().AddMvcOptions(option =>
{
    option.OutputFormatters.Clear();
    option.OutputFormatters.Add(new MessagePackOutputFormatter(MessagePack.MessagePackSerializerOptions.Standard));
    option.InputFormatters.Clear();
    option.InputFormatters.Add(new MessagePackInputFormatter(MessagePack.MessagePackSerializerOptions.Standard));
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();
app.UseRouting();
app.UseCors("CorsPolicy");
app.AddRefDataClient(builder.Configuration);
app.UseFanexData(builder.Configuration, app.Environment);
app.UseMiddleware<ExceptionMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapHealthChecks("/health").AllowAnonymous();
});

if (!app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://0.0.0.0:5000");
}

app.Run();