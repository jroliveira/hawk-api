namespace Hawk.WebApi.Features.Category
{
    using FluentValidation;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
    {
        internal CreateCategoryModelValidator(
            Email email,
            string category,
            IGetCategoryByName getCategoryByName) => this.RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MustAsync(async (name, _) => await getCategoryByName.GetResult(NewGetByIdParam(email, category)) || category.Equals(name))
                .WithMessage("Path category must be equal to body category.");
    }
}
