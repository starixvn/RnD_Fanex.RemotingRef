namespace Fanex.RemotingRef.Service
{
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ModelBinding.Binders;
    using Fanex.RemotingRef.Service.Filters;
    using Fanex.RemotingRef.Service.Utilities;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Add(new MessagePackFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional });

            config.Filters.Add(new GlobalExceptionFilterAttribute());

            var provider = new SimpleModelBinderProvider(typeof(string), new NullOrEmptyStringModelBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
        }
    }
}