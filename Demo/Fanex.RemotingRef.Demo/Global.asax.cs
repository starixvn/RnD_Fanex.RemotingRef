namespace Fanex.RemotingRef.Demo
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using global::RemotingRef;
    using NLog;
    using RefNewData;
    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;

#pragma warning disable SA1649 // File name must match first type name

    public class MvcApplication : HttpApplication
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new Container();
            RefDataTypeContainer.ConnectTo(new Uri(ConfigurationManager.AppSettings["RemotingRefService"]));
            container.Register<IRemotingRefService, RemotingRefService>(Lifestyle.Singleton);
            container.Register<IRefData, RefNewDataHandler>(Lifestyle.Singleton);
            container.Register<IRemotingRefDataAccess, RemotingRefDataAccess>(Lifestyle.Singleton);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();

            logger.Error(CultureInfo.InvariantCulture, Server.GetLastError());
        }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }

#pragma warning restore SA1649 // File name must match first type name
}