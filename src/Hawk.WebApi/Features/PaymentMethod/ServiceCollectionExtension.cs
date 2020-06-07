namespace Hawk.WebApi.Features.PaymentMethod
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePaymentMethod(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string, CreatePaymentMethodModel, Task<ValidationResult>>>(factory => (email, paymentMethod, request) => new CreatePaymentMethodModelValidator(
                email.Get(),
                paymentMethod,
                factory.GetService<IGetPaymentMethodByName>()).ValidateAsync(request));
    }
}
