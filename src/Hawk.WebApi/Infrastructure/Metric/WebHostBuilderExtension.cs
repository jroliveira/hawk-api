namespace Hawk.WebApi.Infrastructure.Metric
{
    using App.Metrics;
    using App.Metrics.Formatters.Prometheus;

    using Microsoft.AspNetCore.Hosting;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureMetric(this IWebHostBuilder @this) => @this
            .ConfigureMetricsWithDefaults(builder =>
            {
                builder.OutputMetrics.AsPrometheusPlainText();
                builder.OutputMetrics.AsPrometheusProtobuf();
            })
            .UseMetricsEndpoints(options =>
            {
                options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                options.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            })
            .UseMetricsWebTracking();
    }
}
