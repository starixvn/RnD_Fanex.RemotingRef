
using Fanex.RemotingRef.NetCore;

var builder = CreateHostBuilder(args);

var host = builder.Build();
host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
            if (!isDevelopment)
            {
                webBuilder.UseUrls("http://0.0.0.0:5000");
            }
        });