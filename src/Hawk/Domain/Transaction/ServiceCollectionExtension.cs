namespace Hawk.Domain.Transaction
{
    using Hawk.Domain.Transaction.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTransaction(this IServiceCollection @this) => @this
            .ConfigureTransactionWithNeo4J();
    }
}
