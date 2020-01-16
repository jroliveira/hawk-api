namespace Hawk.WebApi.Features.Currency
{
    using FluentValidation;

    internal sealed class NewCurrencyModelValidator : AbstractValidator<NewCurrencyModel>
    {
        internal NewCurrencyModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Currency name is required.");
    }
}
