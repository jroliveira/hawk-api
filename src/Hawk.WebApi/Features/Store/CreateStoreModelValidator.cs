namespace Hawk.WebApi.Features.Store
{
    using FluentValidation;

    internal sealed class CreateStoreModelValidator : AbstractValidator<CreateStoreModel>
    {
        internal CreateStoreModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Store name is required.");
    }
}
