namespace Hawk.WebApi.Lib.Validators
{
    using System;
    using FluentValidation;
    using Hawk.WebApi.Models.Transaction;

    internal sealed class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            this.RuleFor(model => model.Date)
                .NotEqual(default(DateTime))
                .WithMessage("Data do pagamento deve ser informado.");

            this.RuleFor(model => model.Value)
                .GreaterThan(0)
                .WithMessage("Valor do pagamento deve ser informado.");

            this.RuleFor(model => model.Currency)
                .NotEmpty()
                .WithMessage("Moeda do pagamento deve ser informado.");
        }
    }
}