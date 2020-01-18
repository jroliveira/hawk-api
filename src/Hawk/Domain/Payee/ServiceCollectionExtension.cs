namespace Hawk.Domain.Payee
{
    using Hawk.Domain.Payee.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePayee(this IServiceCollection @this) => @this
            .ConfigurePayeeWithNeo4J();
    }
}
