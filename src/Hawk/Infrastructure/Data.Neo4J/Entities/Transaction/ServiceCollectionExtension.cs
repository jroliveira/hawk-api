namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using Hawk.Domain.Transaction;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTransactionWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertTransaction, UpsertTransaction>()
            .AddScoped<IDeleteTransaction, DeleteTransaction>()
            .AddScoped<IGetTransactions, GetTransactions>()
            .AddScoped<IGetTransactionById, GetTransactionById>();
    }
}
