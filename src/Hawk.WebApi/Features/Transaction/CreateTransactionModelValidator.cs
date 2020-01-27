namespace Hawk.WebApi.Features.Transaction
{
    using FluentValidation;

    internal sealed class CreateTransactionModelValidator : AbstractValidator<CreateTransactionModel>
    {
        internal CreateTransactionModelValidator()
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Transaction type is required.");

            this.RuleFor(model => model.Payment)
                .NotNull()
                .WithMessage("Transaction payment is required.")
                .SetValidator(new PaymentModelValidator());

            this.RuleFor(model => model.Payee)
                .NotEmpty()
                .WithMessage("Transaction payee is required.");

            this.RuleFor(model => model.Category)
                .NotEmpty()
                .WithMessage("Transaction category is required.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must be at least one tag for transaction.");
        }
    }
}
