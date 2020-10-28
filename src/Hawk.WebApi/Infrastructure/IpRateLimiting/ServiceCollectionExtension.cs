namespace Hawk.WebApi.Infrastructure.IpRateLimiting
{
    using AspNetCoreRateLimit;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureIpRateLimiting(this IServiceCollection @this, IConfiguration configuration) => @this
            .Configure<IpRateLimitOptions>(configuration.GetSection("ipRateLimiting"))
            .Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"))
            .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
            .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    }
}
