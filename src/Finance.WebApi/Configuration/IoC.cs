namespace Finance.WebApi.Configuration
{
    using Finance.Entities;
    using Finance.Entities.Transaction;
    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Filter;
    using Finance.WebApi.Lib.Validators;

    using Http.Query.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Neo4j = Finance.Infrastructure.Data.Neo4j;

    internal static class IoC
    {
        internal static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfigurationRoot configuration)
        {
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
            services.AddSingleton<Neo4j.Queries.Store.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Tag.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetByIdQuery>();

            // Mappings
            services.AddSingleton<Neo4j.Mappings.IMapping<Account>, Neo4j.Mappings.AccountMapping>();
            services.AddSingleton<Neo4j.Mappings.IMapping<Store>, Neo4j.Mappings.Transaction.StoreMapping>();
            services.AddSingleton<Neo4j.Mappings.IMapping<Tag>, Neo4j.Mappings.Transaction.TagMapping>();
            services.AddSingleton<Neo4j.Mappings.IMapping<Transaction>, Neo4j.Mappings.Transaction.TransactionMapping>();

            services.AddSingleton<Neo4j.Mappings.AccountMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.CurrencyMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.MethodMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.PaymentMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.ParcelMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.StoreMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.TagMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.TransactionMapping>();

            // Validators
            services.AddSingleton<AccountValidator>();
            services.AddSingleton<TransactionValidator>();

            return services;
        }
    }
}