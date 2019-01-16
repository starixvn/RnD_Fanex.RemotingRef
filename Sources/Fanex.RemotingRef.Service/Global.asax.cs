#pragma warning disable SA1649 // File name must match first type name

namespace Fanex.RemotingRef.Service
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Web;

    public class WebApiApplication : HttpApplication
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ExceptionHandler.UseConfig(new LoggingConfiguration().GetHttpHeaderInfo());
        }

        protected void Application_Error(object sender, EventArgs e)
            => ExceptionHandler.Handle(Server.GetLastError());

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }
}