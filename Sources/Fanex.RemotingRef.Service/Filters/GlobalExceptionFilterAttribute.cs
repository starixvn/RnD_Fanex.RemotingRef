namespace Fanex.RemotingRef.Service.Filters
{
    using System;
    using System.Globalization;
    using System.Web.Http.Filters;
    using NLog;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            Logger logger = LogManager.GetCurrentClassLogger();

            logger.Error(CultureInfo.InvariantCulture, actionExecutedContext.Exception);
        }
    }
}