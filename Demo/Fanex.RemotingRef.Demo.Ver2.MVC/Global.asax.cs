namespace Fanex.RemotingRef.Demo.Ver2.MVC
{
    using System;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

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

            RefDataTypeContainer.ConnectTo(new Uri(ConfigurationManager.AppSettings["RemotingRefService"]));
        }
    }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static

#pragma warning restore SA1649 // File name must match first type name
}