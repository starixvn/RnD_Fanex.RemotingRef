namespace Fanex.RemotingRef.Service.MVC
{
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Logging.Extensions.Web;

#pragma warning disable SA1649 // File name must match first type name

    public class WebApiApplication : System.Web.HttpApplication
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ExceptionHandler
                .UseConfig(new LoggingConfiguration().GetHttpHeaderInfo());
        }

#pragma warning disable RECS0154 // Parameter is never used
        protected void Application_Error(object sender, EventArgs e)
#pragma warning restore RECS0154 // Parameter is never used
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore SA1649 // File name must match first type name
}