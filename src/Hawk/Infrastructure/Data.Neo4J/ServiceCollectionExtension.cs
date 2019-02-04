namespace Hawk.Infrastructure.Data.Neo4J
{
    using Hawk.Infrastructure.Data.Neo4J.Entities.Account;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Configuration;
    using Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Store;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureNeo4J(this IServiceCollection @this, IConfiguration configuration) => @this
            .AddScoped<Database>()
            .Configure<Configuration>(configuration.GetSection("neo4j"))
            .ConfigureFilterWithNeo4J()
            .ConfigureAccountWithNeo4J()
            .ConfigureConfigurationWithNeo4J()
            .ConfigurePaymentMethodWithNeo4J()
            .ConfigureStoreWithNeo4J()
            .ConfigureTagWithNeo4J()
            .ConfigureTransactionWithNeo4J();
    }
}
