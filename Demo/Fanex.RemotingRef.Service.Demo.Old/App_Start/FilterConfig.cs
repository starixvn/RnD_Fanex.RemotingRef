namespace Fanex.RemotingRef.Service.Demo.Old
{
    using System.Web.Mvc;

#pragma warning disable S1118 // Utility classes should not have public constructors

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

#pragma warning restore S1118 // Utility classes should not have public constructors
}