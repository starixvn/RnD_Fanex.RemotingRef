using System.Web;
using System.Web.Mvc;

namespace Fanex.RemotingRef.Service.Old.Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
