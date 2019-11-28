namespace Hawk.WebApi.Infrastructure.Metric
{
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureMetric(this IServiceCollection @this) => @this
            .Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true)
            .AddMetrics()
            .AddMetricsTrackingMiddleware();
    }
}
