namespace Fanex.RemotingRef.Service.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using App.Metrics;
    using App.Metrics.Logging;
    using App.Metrics.Reporting.InfluxDB;
    using Fanex.RemotingRef.Service.Filters;
    using NSubstitute;
    using Xunit;

    public class MetricsConfigTests
    {
        private const string MetricsEnabledSettingKey = "MetricsEnabled";
        private const string MetricsFlushIntervalSettingKey = "MetricsFlushInterval";

        private readonly NameValueCollection _appSettings;

        public MetricsConfigTests()
        {
            _appSettings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            LogProvider.IsDisabled = true;
        }

        [Fact]
        public void TurnOffMetricsWhenMetricsEnabledInAppSettingsIsMissed()
        {
            // Act.
            var metricsEnabled = MetricsConfig.IsMetricsEnabled(_appSettings);

            // Assert.
            Assert.False(metricsEnabled);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TurnOffMetricsWhenMetricsEnabledInAppSettingsIsNullOrEmpty(string nullOrEmptyConfiguredValue)
        {
            // Arrange.
            _appSettings.Add(MetricsEnabledSettingKey, nullOrEmptyConfiguredValue);

            // Act.
            var metricsEnabled = MetricsConfig.IsMetricsEnabled(_appSettings);

            // Assert.
            Assert.False(metricsEnabled);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("yes")]
        public void TurnOffMetricsWhenMetricsEnabledInAppSettingsTakesNonBooleanValue(string nonBooleanValue)
        {
            // Arrange.
            _appSettings.Add(MetricsEnabledSettingKey, nonBooleanValue);

            // Act.
            var metricsEnabled = MetricsConfig.IsMetricsEnabled(_appSettings);

            // Assert.
            Assert.False(metricsEnabled);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("FALSE")]
        public void TurnOffMetricsWhenMetricsEnabledInAppSettingsTakesAnyFormOfFalse(string falsyValue)
        {
            // Arrange.
            _appSettings.Add(MetricsEnabledSettingKey, falsyValue);

            // Act.
            var metricsEnabled = MetricsConfig.IsMetricsEnabled(_appSettings);

            // Assert.
            Assert.False(metricsEnabled);
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        public void TurnOnMetricsWhenMetricsEnabledInAppSettingsTakesAnyFormOfTrue(string truthyValue)
        {
            // Arrange.
            _appSettings.Add(MetricsEnabledSettingKey, truthyValue);

            // Act.
            var metricsEnabled = MetricsConfig.IsMetricsEnabled(_appSettings);

            // Assert.
            Assert.True(metricsEnabled);
        }

        [Fact]
        public void ReadMetricsFlushIntervalInAppSettings()
        {
            // Arrange.
            _appSettings.Add(MetricsFlushIntervalSettingKey, "0:1:0");

            // Act.
            var actualFlushInterval = MetricsConfig.GetFlushInterval(_appSettings);

            // Assert.
            var expectedFlushInterval = TimeSpan.FromMinutes(1);

            Assert.Equal(expectedFlushInterval, actualFlushInterval);
        }

        [Fact]
        public void TakeDefaultFlushIntervalWhenNotConfigured()
        {
            // Act.
            var actualFlushInterval = MetricsConfig.GetFlushInterval(_appSettings);

            // Assert.
            Assert.Equal(MetricsConfig.DefaultFlushInterval, actualFlushInterval);
        }

        [Fact]
        public void TakeDefaultFlushIntervalWhenConfiguringNonTimeSpanValue()
        {
            // Arrange.
            _appSettings.Add(MetricsFlushIntervalSettingKey, "textual value");

            // Act.
            var actualFlushInterval = MetricsConfig.GetFlushInterval(_appSettings);

            // Assert.
            Assert.Equal(MetricsConfig.DefaultFlushInterval, actualFlushInterval);
        }

        [Fact]
        public void ReadInfluxDbOptionsBasedOnKeysStartWithInfluxDbOptionsPrefixInAppSettings()
        {
            // Arrange.
            const string testingInfluxDb = "http://localhost:8086";
            const string testingDatabase = "Fanex.RemotingRef";

            _appSettings.Add("InfluxDbOptions:BaseUri", testingInfluxDb);
            _appSettings.Add("InfluxDbOptions:Database", testingDatabase);

            // Act.
            var actualInfluxDbOptions = MetricsConfig.ReadInfluxDbOptions(_appSettings);

            // Assert.
            var expectedInfluxDbUri = new Uri(testingInfluxDb);

            Assert.Equal(expectedInfluxDbUri, actualInfluxDbOptions.BaseUri);
            Assert.Equal(testingDatabase, actualInfluxDbOptions.Database);
        }

        [Fact]
        public void UriToInfluxDbTakesNullWhenConfiguringIncorrectUriFormat()
        {
            // Arrange.
            _appSettings.Add("InfluxDbOptions:BaseUri", "localhost");

            // Act.
            var actualInfluxDbOptions = MetricsConfig.ReadInfluxDbOptions(_appSettings);

            // Assert.
            Assert.Null(actualInfluxDbOptions.BaseUri);
        }

        [Fact]
        public void ReportToInfluxDbBasedOnKeysStartWithInfluxDbOptionsInAppSettings()
        {
            // Arrange.
            _appSettings.Add("InfluxDbOptions:BaseUri", "http://localhost:8086");
            _appSettings.Add("InfluxDbOptions:Database", "Fanex.RemotingRef");

            // Act.
            var metrics = MetricsConfig.CreateMetrics(_appSettings);
            var actualReporter = metrics.Reporters.ElementAt(0);

            // Assert.
            Assert.IsType<InfluxDbMetricsReporter>(actualReporter);
        }

        [Fact]
        public void DoNotRegisterMetricsFilterAttributeWhenTurningOffMetrics()
        {
            // Arrange.
            var httpConfig = new HttpConfiguration();

            _appSettings.Add(MetricsEnabledSettingKey, "false");

            // Act.
            MetricsConfig.Register(httpConfig, _appSettings);

            // Assert.
            Assert.Empty(httpConfig.Filters);

            httpConfig.Dispose();
        }

        [Fact]
        public void RegisterMetricsFilterAttributeWhenTurningOnMetrics()
        {
            // Arrange.
            var httpConfig = new HttpConfiguration();

            _appSettings.Add(MetricsEnabledSettingKey, "true");

            // Act.
            MetricsConfig.Register(httpConfig, _appSettings);

            // Assert.
            Assert.IsType<MetricsFilterAttribute>(httpConfig.Filters.ElementAt(0).Instance);

            httpConfig.Dispose();
        }

        [Fact]
        public async Task FlushReportOnceFlushIntervalMet()
        {
            // Arrange.
            var metrics = Substitute.For<IMetricsRoot>();
            var interval = TimeSpan.FromMilliseconds(1);
            var semaphore = new SemaphoreSlim(0, 1);

            metrics.ReportRunner
                .When(caller => caller.RunAllAsync())
                .Do(caller => semaphore.Release());

            // Act.
            var scheduler = MetricsConfig.ScheduleReporting(metrics, interval);

            // Assert.
            var waitingTime = TimeSpan.FromSeconds(30);

            Assert.True(await semaphore.WaitAsync(waitingTime).ConfigureAwait(false));
            metrics.ReportRunner.Received().RunAllAsync();

            scheduler.Dispose();
        }
    }
}