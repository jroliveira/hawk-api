namespace Hawk.WebApi.Features.Payee
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePayee(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string, CreatePayeeModel, Task<ValidationResult>>>(factory => (email, payee, request) => new CreatePayeeModelValidator(
                email.Get(),
                payee,
                factory.GetService<IGetPayeeByName>()).ValidateAsync(request));
    }
}
