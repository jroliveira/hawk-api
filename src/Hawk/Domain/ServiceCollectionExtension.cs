namespace Hawk.Domain
{
    using Hawk.Domain.Account;
    using Hawk.Domain.Configuration;
    using Hawk.Domain.Currency;
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureDomain(this IServiceCollection @this) => @this
            .ConfigureAccount()
            .ConfigureConfiguration()
            .ConfigureCurrency()
            .ConfigurePaymentMethod()
            .ConfigureStore()
            .ConfigureTag()
            .ConfigureTransaction();
    }
}
