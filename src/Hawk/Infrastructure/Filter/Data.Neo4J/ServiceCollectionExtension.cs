namespace Hawk.Infrastructure.Filter.Data.Neo4J
{
    using Hawk.Infrastructure.Filter;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureFilterWithNeo4J(this IServiceCollection @this) => @this
            .AddSingleton<IWhere<string, Http.Query.Filter.Filter>, Where>()
            .AddSingleton<ISkip<int, Http.Query.Filter.Filter>, Skip>()
            .AddSingleton<ILimit<int, Http.Query.Filter.Filter>, Limit>();
    }
}
