namespace Finance.WebApi.Lib.Validators
{
    using System;

    using Finance.WebApi.Models.Transaction;

    using FluentValidation;

    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            this.RuleFor(model => model.Date)
                .NotEqual(default(DateTime))
                .WithMessage("Data do pagamento deve ser informado.");

            this.RuleFor(model => model.Value)
                .GreaterThan(0)
                .WithMessage("Valor do pagamento deve ser informado.");
        }
    }
}