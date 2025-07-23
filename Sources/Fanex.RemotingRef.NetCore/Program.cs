
using Fanex.RemotingRef.NetCore;

ThreadPool.GetMinThreads(out int worker, out int io);
Console.WriteLine($"Default MinThreads: Worker={worker}, IO={io}");

ThreadPool.SetMinThreads(100, 100);

ThreadPool.GetMinThreads(out worker, out io);
Console.WriteLine($"New MinThreads: Worker={worker}, IO={io}");

var builder = CreateHostBuilder(args);

var host = builder.Build();
host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("http://0.0.0.0:5000");
        });