namespace Fanex.RemotingRef.Service.Tests.App_Start.Filters
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using App.Metrics;
    using App.Metrics.Counter;
    using Fanex.RemotingRef.Service.Filters;
    using NSubstitute;
    using Xunit;

    public class MetricsFilterAttributeTests
    {
        private readonly IMetricsRoot _metrics;
        private readonly MetricsFilterAttribute _attribute;

        public MetricsFilterAttributeTests()
        {
            _metrics = Substitute.For<IMetricsRoot>();
            _attribute = new MetricsFilterAttribute(_metrics);
        }

        public static object[][] HttpMethodsExceptGet => new[]
        {
            new[] { HttpMethod.Post },
            new[] { HttpMethod.Put },
            new[] { HttpMethod.Delete },
        };

        [Fact]
        public void NewInstanceWithNullMetricsThrowsArgumentNullException()
        {
            // Arrange.
            var exception = Record.Exception(() => new MetricsFilterAttribute(null));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void MeasurementUnitIsAlwaysRequests()
        {
            // Assert.
            Assert.Equal(Unit.Requests, _attribute.RequestCounter.MeasurementUnit);
        }

        [Fact]
        public void AlwaysResetCounterOnReporting()
        {
            // Assert.
            Assert.True(_attribute.RequestCounter.ResetOnReporting);
        }

        [Theory]
        [MemberData(nameof(HttpMethodsExceptGet))]
        public void OnlyTrackGetRequests(HttpMethod method)
        {
            // Arrange.
            const string applicationName = "TestingApp";
            const string actionName = "BetTypeName";

            var actionContext = FakeHttpActionContext(method, applicationName, actionName);

            // Act.
            _attribute.OnActionExecuting(actionContext);

            // Assert.
            _metrics.Measure.Counter
                .DidNotReceive()
                .Increment(Arg.Any<CounterOptions>(), Arg.Any<MetricTags>(), Arg.Any<string>());
        }

        [Fact]
        public void TrackActionNameAsMetricTags()
        {
            // Arrange.
            const string applicationName = "TestingApp";
            const string actionName = "TeamName";

            var actionContext = FakeHttpActionContext(HttpMethod.Get, applicationName, actionName);

            // Act.
            _attribute.OnActionExecuting(actionContext);

            // Assert.
            _metrics.Measure.Counter
                .DidNotReceive()
                .Increment(
                    Arg.Is(_attribute.RequestCounter),
                    Arg.Is<MetricTags>(tag => tag.Keys.Contains(actionName)),
                    Arg.Any<string>());
        }

        [Fact]
        public void TrackApplicationNameAsMetricItem()
        {
            // Arrange.
            const string applicationName = "TestingApp";
            const string actionName = "LeagueName";

            var actionContext = FakeHttpActionContext(HttpMethod.Get, applicationName, actionName);

            // Act.
            _attribute.OnActionExecuting(actionContext);

            // Assert.
            _metrics.Measure.Counter
                .DidNotReceive()
                .Increment(
                    Arg.Is(_attribute.RequestCounter),
                    Arg.Is<MetricTags>(tag => tag.Keys.Contains(actionName)),
                    Arg.Is(applicationName));
        }

        private HttpActionContext FakeHttpActionContext(HttpMethod method, string applicationName, string actionName)
        {
            var requestUri = new Uri($"http://localhost:8888?applicationName={applicationName}");
            var request = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = method
            };
            var controllerContext = new HttpControllerContext
            {
                Request = request
            };
            var actionDescriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            var actionContext = new HttpActionContext
            {
                ControllerContext = controllerContext,
                ActionDescriptor = actionDescriptor
            };

            actionDescriptor.ActionName.Returns(actionName);

            return actionContext;
        }
    }
}