namespace Fanex.RemotingRef.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using App.Metrics;
    using App.Metrics.Filtering;
    using App.Metrics.Reporting.InfluxDB;
    using App.Metrics.Scheduling;
    using Fanex.RemotingRef.Service.Filters;

    public static class MetricsConfig
    {
        public const string InfluxDbOptionsPrefix = "InfluxDbOptions:";

        public static readonly TimeSpan DefaultFlushInterval = TimeSpan.FromSeconds(5);

        public static bool IsMetricsEnabled(NameValueCollection appSettings)
        {
            const string metricsEnabledSettingKey = "MetricsEnabled";

            var configuredValue = appSettings[metricsEnabledSettingKey];

            return bool.TrueString.Equals(configuredValue, StringComparison.OrdinalIgnoreCase);
        }

        public static TimeSpan GetFlushInterval(NameValueCollection appSettings)
        {
            const string metricsFlushIntervalSettingKey = "MetricsFlushInterval";

            var configuredValue = appSettings[metricsFlushIntervalSettingKey];

            if (TimeSpan.TryParse(configuredValue, out TimeSpan flushInterval))
            {
                return flushInterval;
            }

            return DefaultFlushInterval;
        }

        public static InfluxDbOptions ReadInfluxDbOptions(NameValueCollection appSettings)
        {
            var configuredInfluxDbOptions = FilterConfiguredInfluxDbOptions(appSettings);

            return ConvertToInfluxDbOptions(configuredInfluxDbOptions);
        }

        public static AppMetricsTaskScheduler ScheduleReporting(IMetricsRoot metrics, TimeSpan interval)
        {
            var scheduler = new AppMetricsTaskScheduler(
                interval,
                async () =>
                {
                    await Task.WhenAll(metrics.ReportRunner.RunAllAsync()).ConfigureAwait(false);
                });

            scheduler.Start();

            return scheduler;
        }

        public static IMetricsRoot CreateMetrics(NameValueCollection appSettings)
        {
            var flushInterval = GetFlushInterval(appSettings);
            var influxDbOptions = ReadInfluxDbOptions(appSettings);
            var filter = new MetricsFilter().WhereType(MetricType.Counter);
            var metrics = new MetricsBuilder()
                .Report.ToInfluxDb(options =>
                {
                    options.InfluxDb = influxDbOptions;
                    options.Filter = filter;
                    options.FlushInterval = flushInterval;
                })
                .Build();

            ScheduleReporting(metrics, flushInterval);

            return metrics;
        }

        public static void Register(HttpConfiguration config, NameValueCollection appSettings)
        {
            if (IsMetricsEnabled(appSettings))
            {
                var metrics = CreateMetrics(appSettings);

                config.Filters.Add(new MetricsFilterAttribute(metrics));
            }
        }

        public static void Register(HttpConfiguration config)
            => Register(config, ConfigurationManager.AppSettings);

        private static Dictionary<string, string> FilterConfiguredInfluxDbOptions(NameValueCollection appSettings)
            => appSettings.AllKeys
                .Where(key => key.StartsWith(InfluxDbOptionsPrefix, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(
                    key => key.Substring(InfluxDbOptionsPrefix.Length),
                    key => appSettings[key],
                    StringComparer.OrdinalIgnoreCase);

        private static InfluxDbOptions ConvertToInfluxDbOptions(Dictionary<string, string> configuredOptions)
        {
            var influxDbOptions = new InfluxDbOptions();
            var influxDbOptionsProperties = typeof(InfluxDbOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in influxDbOptionsProperties)
            {
                if (configuredOptions.ContainsKey(property.Name))
                {
                    var propertyType = property.PropertyType;
                    var configuredValue = configuredOptions[property.Name];
                    var settableValue = ChangeType(propertyType, configuredValue);

                    if (settableValue != null)
                    {
                        property.SetValue(influxDbOptions, settableValue, null);
                    }
                }
            }

            return influxDbOptions;
        }

        private static object ChangeType(Type type, string value)
        {
            try
            {
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    return Activator.CreateInstance(type, value);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}