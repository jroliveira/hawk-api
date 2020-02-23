namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using Hawk.Domain.PaymentMethod.Commands;
    using Hawk.Domain.PaymentMethod.Data.Neo4J.Commands;
    using Hawk.Domain.PaymentMethod.Data.Neo4J.Queries;
    using Hawk.Domain.PaymentMethod.Queries;

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
