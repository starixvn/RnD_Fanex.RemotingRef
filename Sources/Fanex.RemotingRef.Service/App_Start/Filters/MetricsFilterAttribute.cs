namespace Fanex.RemotingRef.Service.Filters
{
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using App.Metrics;
    using App.Metrics.Counter;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MetricsFilterAttribute : ActionFilterAttribute
    {
        private const string MetricsName = "Fanex.RemotingRef.Service";

        public MetricsFilterAttribute(IMetricsRoot metrics)
        {
            Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            RequestCounter = new CounterOptions
            {
                Name = MetricsName,
                MeasurementUnit = Unit.Requests,
                ResetOnReporting = true
            };
        }

        public IMetricsRoot Metrics { get; }

        public CounterOptions RequestCounter { get; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Get)
            {
                const string applicationNameParameterName = "applicationName";
                const string actionNameTagName = "ActionName";

                var parameters = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                var actionName = actionContext.ActionDescriptor.ActionName;
                var applicationName = parameters[applicationNameParameterName];
                var tag = new MetricTags(actionNameTagName, actionName);

                Metrics.Measure.Counter.Increment(RequestCounter, tag, applicationName);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}