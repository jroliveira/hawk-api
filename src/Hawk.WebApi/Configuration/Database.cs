namespace Hawk.WebApi.Configuration
{
    using Hawk.Infrastructure.Data.Neo4J;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Database
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<Configuration>(configuration.GetSection("neo4j"));

            return services;
        }
    }
}