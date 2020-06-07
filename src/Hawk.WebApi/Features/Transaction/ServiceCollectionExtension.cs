namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction.Queries;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTransaction(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, CreateTransactionModel, Task<ValidationResult>>>(factory => (email, request) => new CreateTransactionModelValidator(
                email.Get(),
                factory.GetService<IGetCategoryByName>(),
                factory.GetService<IGetCurrencyByCode>(),
                factory.GetService<IGetPayeeByName>(),
                factory.GetService<IGetPaymentMethodByName>(),
                factory.GetService<IGetTransactions>()).ValidateAsync(request));
    }
}
