namespace Hawk.WebApi.Infrastructure.Caching
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureCache(this IServiceCollection @this) => @this
            .AddMemoryCache();
    }
}
