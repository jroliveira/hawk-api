namespace Hawk.WebApi.Features.Currency
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCurrency(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string, CreateCurrencyModel, Task<ValidationResult>>>(factory => (email, currency, request) => new CreateCurrencyModelValidator(
                email.Get(),
                currency,
                factory.GetService<IGetCurrencyByCode>()).ValidateAsync(request));
    }
}
