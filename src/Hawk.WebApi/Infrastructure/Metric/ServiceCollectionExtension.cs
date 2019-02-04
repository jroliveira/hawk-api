namespace Hawk.WebApi.Infrastructure.Metric
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureMetric(this IServiceCollection @this) => @this
            .AddMetrics()
            .AddMetricsTrackingMiddleware();
    }
}
