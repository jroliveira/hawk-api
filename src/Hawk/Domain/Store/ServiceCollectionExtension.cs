namespace Hawk.Domain.Store
{
    using Hawk.Domain.Store.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStore(this IServiceCollection @this) => @this
            .ConfigureStoreWithNeo4J();
    }
}
