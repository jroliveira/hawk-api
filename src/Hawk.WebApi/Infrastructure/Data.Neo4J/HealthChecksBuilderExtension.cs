namespace Hawk.WebApi.Infrastructure.Data.Neo4J
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class HealthChecksBuilderExtension
    {
        internal static IHealthChecksBuilder AddNeo4JCheck(this IHealthChecksBuilder @this) => @this
            .AddCheck<Neo4JHealthCheck>("neo4j", tags: new[] { "live", "ready" });
    }
}
