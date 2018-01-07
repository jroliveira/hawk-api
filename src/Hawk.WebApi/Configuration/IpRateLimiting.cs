namespace Hawk.WebApi.Configuration
{
    using AspNetCoreRateLimit;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class IpRateLimiting
    {
        public static IServiceCollection ConfigureIpRateLimiting(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services
                .AddMemoryCache()
                .Configure<IpRateLimitOptions>(configuration.GetSection("ipRateLimiting"))
                .Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"))
                .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
                .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            return services;
        }
    }
}