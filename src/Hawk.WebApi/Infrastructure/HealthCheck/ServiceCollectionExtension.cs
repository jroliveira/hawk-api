namespace Hawk.WebApi.Infrastructure.HealthCheck
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureHealthCheck(
            this IServiceCollection @this,
            Action<IHealthChecksBuilder>? healthChecksBuilderSetup = null)
        {
            var healthChecksBuilder = @this.AddHealthChecks();

            healthChecksBuilderSetup?.Invoke(healthChecksBuilder);

            return @this;
        }
    }
}
