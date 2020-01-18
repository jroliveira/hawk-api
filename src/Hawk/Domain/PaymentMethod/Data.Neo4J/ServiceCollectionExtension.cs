namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using Hawk.Domain.PaymentMethod;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePaymentMethodWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertPaymentMethod, UpsertPaymentMethod>()
            .AddScoped<IDeletePaymentMethod, DeletePaymentMethod>()
            .AddScoped<IGetPaymentMethods, GetPaymentMethods>()
            .AddScoped<IGetPaymentMethodsByPayee, GetPaymentMethodsByPayee>()
            .AddScoped<IGetPaymentMethodByName, GetPaymentMethodByName>();
    }
}
