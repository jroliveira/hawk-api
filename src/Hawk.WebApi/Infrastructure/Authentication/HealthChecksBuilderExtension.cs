namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class HealthChecksBuilderExtension
    {
        internal static IHealthChecksBuilder AddAuthenticationCheck(
            this IHealthChecksBuilder @this,
            IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return @this;
            }

            return @this
                .AddCheck<AuthorityDnsHealthCheck>("authority", tags: new[] { "ready" });
        }
    }
}
