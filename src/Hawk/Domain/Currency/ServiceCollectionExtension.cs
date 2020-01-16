namespace Hawk.Domain.Currency
{
    using Hawk.Domain.Currency.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCurrency(this IServiceCollection @this) => @this
            .ConfigureCurrencyWithNeo4J();
    }
}
