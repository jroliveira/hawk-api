namespace Hawk.WebApi.Features.Tag
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag.Queries;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTag(this IServiceCollection @this) => @this
            .AddScoped<Func<Try<Email>, string, CreateTagModel, Task<ValidationResult>>>(factory => (email, tag, request) => new CreateTagModelValidator(
                email.Get(),
                tag,
                factory.GetService<IGetTagByName>()).ValidateAsync(request));
    }
}
