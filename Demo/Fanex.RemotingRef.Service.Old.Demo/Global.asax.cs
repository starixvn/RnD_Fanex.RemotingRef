namespace Fanex.RemotingRef.Service.Old.Demo
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Swashbuckle.Application;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalConfiguration.Configuration
            //  .EnableSwagger(c =>
            //  {
            //      c.SingleApiVersion("v1", "SwaggerDemoApi");
            //  })
            //  .EnableSwaggerUi();
        }
    }
}