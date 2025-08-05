using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.DataAccesses;


    public class BaseController : Controller
    {
        protected readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public BaseController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }
    }
}