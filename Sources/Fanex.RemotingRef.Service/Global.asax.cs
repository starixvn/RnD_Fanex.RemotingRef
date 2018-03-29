namespace Fanex.RemotingRef.Service.MVC
{
    using System;
    using System.Globalization;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NLog;

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
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            logger.Error(CultureInfo.InvariantCulture, Server.GetLastError());
        }
    }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore SA1649 // File name must match first type name
}