namespace Fanex.RemotingRef.Service.Demo.Old
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

#pragma warning disable SA1649 // File name must match first type name
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore SA1649 // File name must match first type name
}