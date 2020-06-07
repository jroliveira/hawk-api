namespace Hawk.WebApi.Features.Configuration
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureConfiguration(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, CreateConfigurationModel, Task<ValidationResult>>>(factory => (email, request) => new CreateConfigurationModelValidator(
                email.Get(),
                factory.GetService<IGetCategoryByName>(),
                factory.GetService<IGetCurrencyByCode>(),
                factory.GetService<IGetPayeeByName>(),
                factory.GetService<IGetPaymentMethodByName>()).ValidateAsync(request));
    }
}
