namespace Hawk.Domain.Account
{
    using Hawk.Domain.Account.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAccount(this IServiceCollection @this) => @this
            .ConfigureAccountWithNeo4J();
    }
}
