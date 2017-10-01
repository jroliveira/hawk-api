namespace Finance.WebApi.Configuration
{
    using Finance.Infrastructure;
    using Finance.Infrastructure.Filter;
    using Finance.WebApi.GraphQl.Queries;
    using Finance.WebApi.GraphQl.Schemas;
    using Finance.WebApi.Lib.Validators;

    using Http.Query.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Neo4j = Finance.Infrastructure.Data.Neo4j;

    internal static class IoC
    {
        internal static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<PartialUpdater>();

            // Filters
            services.AddSingleton<IWhere<string, Filter>, Neo4j.Filter.Where>();
            services.AddSingleton<ISkip<int, Filter>, Neo4j.Filter.Skip>();
            services.AddSingleton<ILimit<int, Filter>, Neo4j.Filter.Limit>();

            // Commands
            services.AddSingleton<Neo4j.Commands.Account.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Transaction.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Transaction.ExcludeCommand>();
            services.AddSingleton<Neo4j.Commands.Currency.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.PaymentMethod.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Store.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Tag.CreateCommand>();

            // Queries
            services.AddSingleton<Neo4j.Queries.Account.GetByEmailQuery>();
            services.AddSingleton<Neo4j.Queries.PaymentMethod.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.PaymentMethod.GetAllByStoreQuery>();
            services.AddSingleton<Neo4j.Queries.Store.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Tag.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Tag.GetAllByStoreQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetByIdQuery>();

            // Mappings
            services.AddSingleton<Neo4j.Mappings.AccountMapping>();
            services.AddSingleton<Neo4j.Mappings.Payment.CurrencyMapping>();
            services.AddSingleton<Neo4j.Mappings.Payment.MethodMapping>();
            services.AddSingleton<Neo4j.Mappings.Payment.PaymentMapping>();
            services.AddSingleton<Neo4j.Mappings.ParcelMapping>();
            services.AddSingleton<Neo4j.Mappings.StoreMapping>();
            services.AddSingleton<Neo4j.Mappings.TagMapping>();
            services.AddSingleton<Neo4j.Mappings.TransactionMapping>();

            // Reports
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.Store.GetQuery>();
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.Tag.GetQuery>();
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.ItemMapping>();
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.TransactionsMapping>();

            // Validators
            services.AddSingleton<AccountValidator>();
            services.AddSingleton<TransactionValidator>();

            // GraphQL
            services.AddSingleton<TransactionQuery>();
            services.AddSingleton<TransactionSchema>();
            services.AddSingleton<Neo4j.GraphQl.GetQuery>();
            services.AddSingleton<Neo4j.GraphQl.GetAllQuery>();

            return services;
        }
    }
}