namespace Hawk.WebApi.Features.Transaction
{
    using System;

    using FluentValidation;

    internal sealed class PaymentModelValidator : AbstractValidator<PaymentModel>
    {
        internal PaymentModelValidator()
        {
            this.RuleFor(model => model.Value)
                .GreaterThan(0)
                .WithMessage("Payment value is required.");

            this.RuleFor(model => model.Date)
                .NotEqual(default(DateTime))
                .WithMessage("Payment date is required.");

            this.RuleFor(model => model.Method)
                .NotEmpty()
                .WithMessage("Payment method is required.");

            this.RuleFor(model => model.Currency)
                .NotEmpty()
                .WithMessage("Payment currency is required.");
        }
    }
}
