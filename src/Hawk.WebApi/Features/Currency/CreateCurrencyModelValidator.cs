namespace Hawk.WebApi.Features.Currency
{
    using FluentValidation;

    internal sealed class CreateCurrencyModelValidator : AbstractValidator<CreateCurrencyModel>
    {
        internal CreateCurrencyModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Currency name is required.");
    }
}
