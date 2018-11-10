namespace Hawk.WebApi.Configuration
{
    using AspNetCoreRateLimit;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class IpRateLimiting
    {
        internal static IServiceCollection ConfigureIpRateLimiting(
            this IServiceCollection @this,
            IConfiguration configuration)
        {
            @this
                .AddMemoryCache()
                .Configure<IpRateLimitOptions>(configuration.GetSection("ipRateLimiting"))
                .Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"))
                .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
                .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            return @this;
        }
    }
}
