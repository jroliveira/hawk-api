namespace Hawk.WebApi.Features.Transaction
{
    using System;

    using FluentValidation;

    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.WebApi.Features.Shared.Money;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class PaymentModelValidator : AbstractValidator<PaymentModel>
    {
        internal PaymentModelValidator(
            Email email,
            IGetCurrencyByCode getCurrencyByCode,
            IGetPaymentMethodByName getPaymentMethodByName)
        {
            this.RuleFor(model => model.Cost)
                .NotNull()
                .WithMessage("Cost is required.")
                .SetValidator(new MoneyModelValidator(email, getCurrencyByCode));

            this.RuleFor(model => model.Date)
                .NotEqual(default(DateTime))
                .WithMessage("Payment date is required.");

            this.RuleFor(model => model.Method)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MustAsync(async (method, _) => await getPaymentMethodByName.GetResult(NewGetByIdParam(email, method)))
                .WithMessage("Payment method not found.");
        }
    }
}
