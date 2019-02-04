namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using Hawk.Domain.PaymentMethod;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePaymentMethodWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IGetPaymentMethods, GetPaymentMethods>()
            .AddScoped<IGetPaymentMethodsByStore, GetPaymentMethodsByStore>();
    }
}
