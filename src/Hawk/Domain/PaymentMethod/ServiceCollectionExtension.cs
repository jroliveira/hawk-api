namespace Hawk.Domain.PaymentMethod
{
    using Hawk.Domain.PaymentMethod.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePaymentMethod(this IServiceCollection @this) => @this
            .ConfigurePaymentMethodWithNeo4J();
    }
}
