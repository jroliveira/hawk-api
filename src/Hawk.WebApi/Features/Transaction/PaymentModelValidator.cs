namespace Hawk.WebApi.Features.Transaction
{
    using System;

    using FluentValidation;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class PaymentModelValidator : AbstractValidator<PaymentModel>
    {
        internal PaymentModelValidator(
            Email email,
            IGetCurrencyByName getCurrencyByName,
            IGetPaymentMethodByName getPaymentMethodByName)
        {
            this.RuleFor(model => model.Value)
                .GreaterThan(0)
                .WithMessage("Payment value is required.");

            this.RuleFor(model => model.Date)
                .NotEqual(default(DateTime))
                .WithMessage("Payment date is required.");

            this.RuleFor(model => model.Method)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MustAsync(async (method, _) => await getPaymentMethodByName.GetResult(NewGetByIdParam(email, method)))
                .WithMessage("Payee not found.");

            this.RuleFor(model => model.Currency)
                .NotEmpty()
                .WithMessage("Currency is required.")
                .MustAsync(async (currency, _) => await getCurrencyByName.GetResult(NewGetByIdParam(email, currency)))
                .WithMessage("Payee not found.");
        }
    }
}
