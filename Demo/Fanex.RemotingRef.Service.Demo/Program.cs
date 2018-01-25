namespace Fanex.RemotingRef.Service.Demo
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using NLog.Web;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable S1118 // Utility classes should not have public constructors

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
              .UseKestrel()
                .UseSetting("detailedErrors", "true")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseNLog()
                .CaptureStartupErrors(true)
                .Build();
    }

#pragma warning restore S1118 // Utility classes should not have public constructors

#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}