﻿namespace Hawk.WebApi.Lib.Validators
{
    using FluentValidation;
    using Hawk.WebApi.Models.Transaction.Post;

    internal sealed class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Tipo da transação deve ser informado.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Deve ser uma informado pelo menos uma tag para a transação.");

            this.RuleFor(model => model.Payment)
                .SetValidator(new PaymentValidator());
        }
    }
}
