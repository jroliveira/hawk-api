namespace Hawk.WebApi.Features.Shared.Money
{
    using FluentValidation;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;

    internal sealed class MoneyModelValidator : AbstractValidator<MoneyModel>
    {
        internal MoneyModelValidator(
            Email email,
            IGetCurrencyByCode getCurrencyByCode)
        {
            this.RuleFor(model => model.Value)
                .GreaterThan(0)
                .WithMessage("Money value is required.");

            this.RuleFor(model => model.Currency)
                .NotNull()
                .WithMessage("Currency is required.")
                .SetValidator(new CurrencyModelValidator(email, getCurrencyByCode));
        }
    }
}
