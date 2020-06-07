namespace Hawk.WebApi.Features.Currency
{
    using FluentValidation;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreateCurrencyModelValidator : AbstractValidator<CreateCurrencyModel>
    {
        internal CreateCurrencyModelValidator(
            Email email,
            string currency,
            IGetCurrencyByCode getCurrencyByCode)
        {
            this.RuleFor(model => model.Code)
                .NotEmpty()
                .WithMessage("Currency code is required.")
                .MustAsync(async (code, _) => await getCurrencyByCode.GetResult(NewGetByIdParam(email, currency)) || currency.Equals(code))
                .WithMessage("Path currency must be equal to body currency.");

            this.RuleFor(model => model.Symbol)
                .NotEmpty()
                .WithMessage("Currency symbol is required.");
        }
    }
}
