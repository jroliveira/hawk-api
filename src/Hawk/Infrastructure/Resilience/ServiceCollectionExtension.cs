namespace Hawk.Infrastructure.Resilience
{
    using Hawk.Infrastructure.Resilience.Configurations;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureResilience(this IServiceCollection @this, IConfiguration configuration) => @this
            .Configure<ResilienceConfiguration>(configuration.GetSection("resilience"))
            .AddSingleton<ResiliencePolicy>();
    }
}
