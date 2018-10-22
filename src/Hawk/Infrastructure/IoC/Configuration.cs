namespace Hawk.Infrastructure.IoC
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Configuration
    {
        public static IServiceCollection ConfigureIoC(this IServiceCollection @this, IConfigurationRoot configuration)
        {
            // Neo4j
            @this.AddScoped<Data.Neo4J.Database>();
            @this.Configure<Data.Neo4J.Configuration>(configuration.GetSection("neo4j"));

            // Filters
            @this.AddSingleton<IWhere<string, Filter>, Data.Neo4J.Filter.Where>();
            @this.AddSingleton<ISkip<int, Filter>, Data.Neo4J.Filter.Skip>();
            @this.AddSingleton<ILimit<int, Filter>, Data.Neo4J.Filter.Limit>();

            // Commands
            @this.AddScoped<Domain.Commands.Account.ICreateCommand, Data.Neo4J.Commands.Account.CreateCommand>();
            @this.AddScoped<Domain.Commands.Transaction.ICreateCommand, Data.Neo4J.Commands.Transaction.CreateCommand>();
            @this.AddScoped<Domain.Commands.Transaction.IExcludeCommand, Data.Neo4J.Commands.Transaction.ExcludeCommand>();
            @this.AddScoped<Domain.Commands.Store.ICreateCommand, Data.Neo4J.Commands.Store.CreateCommand>();
            @this.AddScoped<Domain.Commands.Store.IExcludeCommand, Data.Neo4J.Commands.Store.ExcludeCommand>();

            // Queries
            @this.AddScoped<Domain.Queries.Account.IGetByEmailQuery, Data.Neo4J.Queries.Account.GetByEmailQuery>();
            @this.AddScoped<Domain.Queries.PaymentMethod.IGetAllQuery, Data.Neo4J.Queries.PaymentMethod.GetAllQuery>();
            @this.AddScoped<Domain.Queries.PaymentMethod.IGetAllByStoreQuery, Data.Neo4J.Queries.PaymentMethod.GetAllByStoreQuery>();
            @this.AddScoped<Domain.Queries.Store.IGetAllQuery, Data.Neo4J.Queries.Store.GetAllQuery>();
            @this.AddScoped<Domain.Queries.Store.IGetByNameQuery, Data.Neo4J.Queries.Store.GetByNameQuery>();
            @this.AddScoped<Domain.Queries.Tag.IGetAllQuery, Data.Neo4J.Queries.Tag.GetAllQuery>();
            @this.AddScoped<Domain.Queries.Tag.IGetAllByStoreQuery, Data.Neo4J.Queries.Tag.GetAllByStoreQuery>();
            @this.AddScoped<Domain.Queries.Transaction.IGetAllQuery, Data.Neo4J.Queries.Transaction.GetAllQuery>();
            @this.AddScoped<Domain.Queries.Transaction.IGetByIdQuery, Data.Neo4J.Queries.Transaction.GetByIdQuery>();

            return @this;
        }
    }
}
