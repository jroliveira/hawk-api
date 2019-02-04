namespace Hawk.Infrastructure.Data.Neo4J.Entities.Configuration
{
    using Hawk.Domain.Configuration;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureConfigurationWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertConfiguration, UpsertConfiguration>()
            .AddScoped<IGetConfigurationByDescription, GetConfigurationByDescription>();
    }
}
