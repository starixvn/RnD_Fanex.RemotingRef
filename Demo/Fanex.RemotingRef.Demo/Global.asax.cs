namespace Fanex.RemotingRef.Demo
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using global::RemotingRef;
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

            container.Register<IUserService, UserService>(Lifestyle.Singleton);
            container.Register<ISecurityService, SecurityService>(Lifestyle.Singleton);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }

#pragma warning restore SA1649 // File name must match first type name
}