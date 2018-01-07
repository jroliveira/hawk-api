namespace Hawk.Infrastructure.IoC
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using Microsoft.Extensions.DependencyInjection;

    public static class Configuration
    {
        public static IServiceCollection ConfigureIoC(this IServiceCollection services)
        {
            // Neo4j
            services.AddSingleton<Data.Neo4J.Database>();
            services.AddSingleton<Data.Neo4J.GetScript>();

            // Filters
            services.AddSingleton<IWhere<string, Filter>, Data.Neo4J.Filter.Where>();
            services.AddSingleton<ISkip<int, Filter>, Data.Neo4J.Filter.Skip>();
            services.AddSingleton<ILimit<int, Filter>, Data.Neo4J.Filter.Limit>();

            // Commands
            services.AddSingleton<Domain.Commands.Account.ICreateCommand, Data.Neo4J.Commands.Account.CreateCommand>();
            services.AddSingleton<Domain.Commands.Transaction.ICreateCommand, Data.Neo4J.Commands.Transaction.CreateCommand>();
            services.AddSingleton<Domain.Commands.Transaction.IExcludeCommand, Data.Neo4J.Commands.Transaction.ExcludeCommand>();

            // Queries
            services.AddSingleton<Domain.Queries.Account.IGetByEmailQuery, Data.Neo4J.Queries.Account.GetByEmailQuery>();
            services.AddSingleton<Domain.Queries.PaymentMethod.IGetAllQuery, Data.Neo4J.Queries.PaymentMethod.GetAllQuery>();
            services.AddSingleton<Domain.Queries.PaymentMethod.IGetAllByStoreQuery, Data.Neo4J.Queries.PaymentMethod.GetAllByStoreQuery>();
            services.AddSingleton<Domain.Queries.Store.IGetAllQuery, Data.Neo4J.Queries.Store.GetAllQuery>();
            services.AddSingleton<Domain.Queries.Tag.IGetAllQuery, Data.Neo4J.Queries.Tag.GetAllQuery>();
            services.AddSingleton<Domain.Queries.Tag.IGetAllByStoreQuery, Data.Neo4J.Queries.Tag.GetAllByStoreQuery>();
            services.AddSingleton<Domain.Queries.Transaction.IGetAllQuery, Data.Neo4J.Queries.Transaction.GetAllQuery>();
            services.AddSingleton<Domain.Queries.Transaction.IGetByIdQuery, Data.Neo4J.Queries.Transaction.GetByIdQuery>();

            // Mappings
            services.AddSingleton<Data.Neo4J.Mappings.AccountMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.Payment.CurrencyMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.Payment.MethodMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.Payment.PayMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.Payment.PriceMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.ParcelMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.StoreMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.TagMapping>();
            services.AddSingleton<Data.Neo4J.Mappings.TransactionMapping>();

            // Reports
            services.AddSingleton<Reports.AmountGroupByStore.IGetQuery, Reports.AmountGroupByStore.GetQuery>();
            services.AddSingleton<Reports.AmountGroupByTag.IGetQuery, Reports.AmountGroupByTag.GetQuery>();
            services.AddSingleton<Reports.Mappings.ItemMapping>();
            services.AddSingleton<Reports.Mappings.TransactionsMapping>();
            services.AddSingleton<Reports.GetScript>();

            return services;
        }
    }
}