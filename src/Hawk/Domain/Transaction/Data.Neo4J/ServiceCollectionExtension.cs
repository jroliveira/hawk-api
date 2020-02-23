namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using Hawk.Domain.Transaction.Commands;
    using Hawk.Domain.Transaction.Data.Neo4J.Commands;
    using Hawk.Domain.Transaction.Data.Neo4J.Queries;
    using Hawk.Domain.Transaction.Queries;

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
