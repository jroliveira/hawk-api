namespace Hawk.Infrastructure.Filter
{
    using Hawk.Infrastructure.Filter.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureFilter(this IServiceCollection @this) => @this
            .ConfigureFilterWithNeo4J();
    }
}
