namespace Hawk.WebApi.Features.Transaction
{
    using FluentValidation;

    internal sealed class NewTransactionModelValidator : AbstractValidator<NewTransactionModel>
    {
        internal NewTransactionModelValidator()
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Transaction type is required.");

            this.RuleFor(model => model.Payment)
                .SetValidator(new PaymentModelValidator());

            this.RuleFor(model => model.Store)
                .NotEmpty()
                .WithMessage("Transaction store is required.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must be at least one tag for transaction.");
        }
    }
}
