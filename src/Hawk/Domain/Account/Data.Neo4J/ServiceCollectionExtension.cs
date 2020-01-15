namespace Hawk.Domain.Account.Data.Neo4J
{
    using Hawk.Domain.Account;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAccountWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertAccount, UpsertAccount>()
            .AddScoped<IGetAccountByEmail, GetAccountByEmail>();
    }
}
