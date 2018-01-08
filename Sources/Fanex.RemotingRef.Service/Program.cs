namespace Fanex.RemotingRef.Service
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

#pragma warning disable S1118 // Utility classes should not have public constructors

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }

#pragma warning restore S1118 // Utility classes should not have public constructors
}