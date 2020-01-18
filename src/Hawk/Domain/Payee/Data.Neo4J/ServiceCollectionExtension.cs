namespace Hawk.Domain.Payee.Data.Neo4J
{
    using Hawk.Domain.Payee;

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
