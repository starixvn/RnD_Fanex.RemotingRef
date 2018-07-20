#pragma warning disable SA1649 // File name must match first type name

namespace Fanex.RemotingRef.Service.MVC
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using Fanex.Logging.Extensions.Web;

    public class WebApiApplication : HttpApplication
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ExceptionHandler.UseConfig(new LoggingConfiguration().GetHttpHeaderInfo());
        }

        protected void Application_Error(object sender, EventArgs e)
            => ExceptionHandler.Handle(Server.GetLastError());

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }
}