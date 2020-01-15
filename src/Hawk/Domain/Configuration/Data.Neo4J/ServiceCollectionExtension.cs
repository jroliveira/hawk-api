namespace Hawk.Domain.Configuration.Data.Neo4J
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
