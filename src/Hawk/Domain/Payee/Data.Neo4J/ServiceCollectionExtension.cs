namespace Hawk.Domain.Payee.Data.Neo4J
{
    using Hawk.Domain.Payee.Commands;
    using Hawk.Domain.Payee.Data.Neo4J.Commands;
    using Hawk.Domain.Payee.Data.Neo4J.Queries;
    using Hawk.Domain.Payee.Queries;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePayeeWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertPayee, UpsertPayee>()
            .AddScoped<IDeletePayee, DeletePayee>()
            .AddScoped<IGetPayees, GetPayees>()
            .AddScoped<IGetPayeeByName, GetPayeeByName>();
    }
}
