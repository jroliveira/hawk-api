namespace Hawk.WebApi.Configuration
{
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Filter;
    using Hawk.WebApi.GraphQl.Queries;
    using Hawk.WebApi.GraphQl.Schemas;
    using Hawk.WebApi.Lib.Validators;

    using Http.Query.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Neo4j = Hawk.Infrastructure.Data.Neo4j;

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
            services.AddSingleton<Neo4j.Mappings.TransactionsMapping>();

            // Reports
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.Store.GetQuery>();
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.Tag.GetQuery>();
            services.AddSingleton<Neo4j.Reports.GetAmountGroupBy.ItemMapping>();

            // Validators
            services.AddSingleton<AccountValidator>();
            services.AddSingleton<TransactionValidator>();

            // GraphQL
            services.AddSingleton<TransactionQuery>();
            services.AddSingleton<StoreQuery>();
            services.AddSingleton<HawkQuery>();
            services.AddSingleton<HawkSchema>();
            services.AddSingleton<Neo4j.GraphQl.Transaction.GetQuery>();
            services.AddSingleton<Neo4j.GraphQl.Transaction.GetAllQuery>();
            services.AddSingleton<Neo4j.GraphQl.Store.GetAllQuery>();

            return services;
        }
    }
}