namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using Hawk.Domain.Configuration.Commands;
    using Hawk.Domain.Configuration.Data.Neo4J.Commands;
    using Hawk.Domain.Configuration.Data.Neo4J.Queries;
    using Hawk.Domain.Configuration.Queries;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureConfigurationWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertConfiguration, UpsertConfiguration>()
            .AddScoped<IDeleteConfiguration, DeleteConfiguration>()
            .AddScoped<IGetConfigurations, GetConfigurations>()
            .AddScoped<IGetConfigurationByDescription, GetConfigurationByDescription>();
    }
}
