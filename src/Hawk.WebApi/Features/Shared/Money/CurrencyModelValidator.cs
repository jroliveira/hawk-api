namespace Hawk.WebApi.Features.Shared.Money
{
    using FluentValidation;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CurrencyModelValidator : AbstractValidator<CurrencyModel>
    {
        internal CurrencyModelValidator(
            Email email,
            IGetCurrencyByCode getCurrencyByCode) => this.RuleFor(model => model.Code)
                .NotEmpty()
                .WithMessage("Currency code is required.")
                .MustAsync(async (currency, _) => await getCurrencyByCode.GetResult(NewGetByIdParam(email, currency)))
                .WithMessage("Currency not found.");
    }
}
