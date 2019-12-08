namespace Hawk.WebApi.Infrastructure.Tracing
{
    using Hawk.Infrastructure.Tracing.Configurations;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseTracing(this IApplicationBuilder @this, IConfiguration configuration)
        {
            var tracingConfig = configuration
                .GetSection("tracing")
                .Get<TracingConfiguration>();

            if (!tracingConfig.IsEnabled())
            {
                return @this;
            }

            return @this
                .UseMiddleware<RequestMonitoringMiddleware>();
        }
    }
}
