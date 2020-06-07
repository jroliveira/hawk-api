namespace Hawk.WebApi.Features.Category
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCategory(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string, CreateCategoryModel, Task<ValidationResult>>>(factory => (email, category, request) => new CreateCategoryModelValidator(
                email.Get(),
                category,
                factory.GetService<IGetCategoryByName>()).ValidateAsync(request));
    }
}
