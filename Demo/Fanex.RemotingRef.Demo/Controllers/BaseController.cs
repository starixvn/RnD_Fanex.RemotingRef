using Microsoft.AspNetCore.Mvc;
using RefClient.Factory;
using RefClient.Service.IService;

namespace Fanex.RemotingRef.Demo.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IRemotingRefService _remotingRefService;
        protected readonly IBetRefClientService twRemotingRefService = RefClientFactory.GetBetRefClientService();

        public BaseController(IRemotingRefService service)
        {
            _remotingRefService = service;
        }

        protected T SafeExecute<T>(Func<T> action, string operationName, T defaultValue = default(T))
        {
            return action();
        }
    }
}