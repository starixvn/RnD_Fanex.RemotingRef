ThreadPool.SetMinThreads(100, 100);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapHealthChecks("/health").AllowAnonymous();
});

builder.WebHost.UseUrls("https://0.0.0.0:5000");
app.Run();