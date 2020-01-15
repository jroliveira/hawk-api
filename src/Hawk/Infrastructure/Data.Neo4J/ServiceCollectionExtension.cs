namespace Hawk.Infrastructure.Data.Neo4J
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureNeo4J(this IServiceCollection @this, IConfiguration configuration) => @this
            .AddScoped<Neo4JConnection>()
            .Configure<Neo4JConfiguration>(configuration.GetSection("neo4j"));
    }
}
