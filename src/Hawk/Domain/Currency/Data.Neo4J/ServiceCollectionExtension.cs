namespace Hawk.Domain.Currency.Data.Neo4J
{
    using Hawk.Domain.Currency;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCurrencyWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertCurrency, UpsertCurrency>()
            .AddScoped<IDeleteCurrency, DeleteCurrency>()
            .AddScoped<IGetCurrencies, GetCurrencies>()
            .AddScoped<IGetCurrencyByName, GetCurrencyByName>();
    }
}
