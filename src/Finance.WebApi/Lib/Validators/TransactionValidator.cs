namespace Finance.WebApi.Lib.Validators
{
    using System.Linq;

    using Finance.WebApi.Models.Transaction.Post;

    using FluentValidation;

    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Tipo da transação deve ser informado.");

            this.RuleFor(model => model.Tags.Count())
                .GreaterThan(0)
                .WithMessage("Deve ser uma informado pelo menos uma tag para a transação.");

            this.RuleFor(model => model.Parcel)
                .SetValidator(new ParcelValidator())
                .When(model => model.Parcel != null);

            this.RuleFor(model => model.Payment)
                .SetValidator(new PaymentValidator());
        }
    }
}
