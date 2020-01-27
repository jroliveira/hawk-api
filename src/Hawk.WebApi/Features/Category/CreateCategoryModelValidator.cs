namespace Hawk.WebApi.Features.Category
{
    using FluentValidation;

    internal sealed class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
    {
        internal CreateCategoryModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Category name is required.");
    }
}
