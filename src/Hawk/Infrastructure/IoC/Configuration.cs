namespace Hawk.Infrastructure.IoC
{
    using Hawk.Domain.Account;
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Account;
    using Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Store;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Transaction;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Configuration
    {
        public static IServiceCollection ConfigureIoC(this IServiceCollection @this, IConfiguration configuration)
        {
            // Neo4j
            @this.AddScoped<Data.Neo4J.Database>();
            @this.Configure<Data.Neo4J.Configuration>(configuration.GetSection("neo4j"));

            // Filters
            @this.AddSingleton<IWhere<string, Filter>, Data.Neo4J.Filter.Where>();
            @this.AddSingleton<ISkip<int, Filter>, Data.Neo4J.Filter.Skip>();
            @this.AddSingleton<ILimit<int, Filter>, Data.Neo4J.Filter.Limit>();

            // Commands
            @this.AddScoped<IUpsertAccount, UpsertAccount>();
            @this.AddScoped<IUpsertTransaction, UpsertTransaction>();
            @this.AddScoped<IDeleteTransaction, DeleteTransaction>();

            // Queries
            @this.AddScoped<IGetAccountByEmail, GetAccountByEmail>();
            @this.AddScoped<IGetPaymentMethods, GetPaymentMethods>();
            @this.AddScoped<IGetPaymentMethodsByStore, GetPaymentMethodsByStore>();
            @this.AddScoped<IGetStores, GetStores>();
            @this.AddScoped<IGetStoreByName, GetStoreByName>();
            @this.AddScoped<IGetTags, GetTags>();
            @this.AddScoped<IGetTagsByStore, GetTagsByStore>();
            @this.AddScoped<IGetTransactions, GetTransactions>();
            @this.AddScoped<IGetTransactionById, GetTransactionById>();

            return @this;
        }
    }
}
