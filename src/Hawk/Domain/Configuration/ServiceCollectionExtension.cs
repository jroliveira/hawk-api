namespace Hawk.Domain.Configuration
{
    using Hawk.Domain.Configuration.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureConfiguration(this IServiceCollection @this) => @this
            .ConfigureConfigurationWithNeo4J();
    }
}
