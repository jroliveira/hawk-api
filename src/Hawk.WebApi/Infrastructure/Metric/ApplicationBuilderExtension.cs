namespace Hawk.WebApi.Infrastructure.Metric
{
    using Microsoft.AspNetCore.Builder;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseMetric(this IApplicationBuilder @this) => @this
            .UseMetricsAllMiddleware();
    }
}
