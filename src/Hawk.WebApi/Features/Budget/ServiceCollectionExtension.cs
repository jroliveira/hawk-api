namespace Hawk.WebApi.Features.Budget
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Budget.Queries;
    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureBudget(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string?, CreateBudgetModel, Task<ValidationResult>>>(factory => (email, id, request) => new CreateBudgetModelValidator(
                email.Get(),
                id,
                factory.GetService<IGetBudgets>(),
                factory.GetService<IGetCategoryByName>(),
                factory.GetService<IGetCurrencyByCode>()).ValidateAsync(request));
    }
}
