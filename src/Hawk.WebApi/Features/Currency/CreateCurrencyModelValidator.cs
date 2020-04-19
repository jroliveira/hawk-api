namespace Hawk.WebApi.Features.Currency
{
    using FluentValidation;

    internal sealed class CreateCurrencyModelValidator : AbstractValidator<CreateCurrencyModel>
    {
        internal CreateCurrencyModelValidator()
        {
            this.RuleFor(model => model.Code)
                .NotEmpty()
                .WithMessage("Currency code is required.");

            this.RuleFor(model => model.Symbol)
                .NotEmpty()
                .WithMessage("Currency symbol is required.");
        }
    }
}
