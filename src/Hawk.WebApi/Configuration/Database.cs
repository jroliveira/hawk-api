namespace Hawk.WebApi.Configuration
{
    using Hawk.Infrastructure.Data.Neo4J;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Database
    {
        internal static IServiceCollection ConfigureDatabase(this IServiceCollection @this, IConfigurationRoot configuration)
        {
            @this.Configure<Configuration>(configuration.GetSection("neo4j"));

            return @this;
        }
    }
}
