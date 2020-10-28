namespace Hawk.Infrastructure.Data.Neo4J
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureNeo4J(this IServiceCollection @this, IConfiguration configuration) => @this
            .Configure<Neo4JConfiguration>(configuration.GetSection("neo4j"))
            .AddScoped<ICheckNeo4J, CheckNeo4J>()
            .AddScoped<Neo4JConnection>();
    }
}
