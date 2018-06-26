namespace Fanex.RemotingRef.Service.Filters
{
    using System;
    using System.Diagnostics;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Logging.Extensions.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class TrackingRequestFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            ExceptionHandler.Handle(new Exception("Logging OnActionExecuting"), TraceEventType.Information);
        }
    }
}