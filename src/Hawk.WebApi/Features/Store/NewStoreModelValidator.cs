namespace Hawk.WebApi.Features.Store
{
    using FluentValidation;

    internal sealed class NewStoreModelValidator : AbstractValidator<NewStoreModel>
    {
        internal NewStoreModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Store name is required.");
    }
}
