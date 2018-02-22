using System.Web;
using System.Web.Mvc;

namespace Fanex.RemotingRef.Service.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
