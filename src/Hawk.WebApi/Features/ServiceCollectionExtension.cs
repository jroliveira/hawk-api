namespace Hawk.WebApi.Features
{
    using Hawk.WebApi.Features.Budget;
    using Hawk.WebApi.Features.Category;
    using Hawk.WebApi.Features.Configuration;
    using Hawk.WebApi.Features.Currency;
    using Hawk.WebApi.Features.Payee;
    using Hawk.WebApi.Features.PaymentMethod;
    using Hawk.WebApi.Features.Tag;
    using Hawk.WebApi.Features.Transaction;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureFeature(this IServiceCollection @this) => @this
            .ConfigureBudget()
            .ConfigureCategory()
            .ConfigureConfiguration()
            .ConfigureCurrency()
            .ConfigurePayee()
            .ConfigurePaymentMethod()
            .ConfigureTag()
            .ConfigureTransaction();
    }
}
