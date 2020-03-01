namespace Hawk.WebApi.Features.Transaction
{
    using System.Linq;

    using FluentValidation;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction.Queries;

    using Http.Query.Filter.Client;
    using Http.Query.Filter.Client.Filters.Condition;

    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreateTransactionModelValidator : AbstractValidator<CreateTransactionModel>
    {
        internal CreateTransactionModelValidator(
            Email email,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByName getCurrencyByName,
            IGetPayeeByName getPayeeByName,
            IGetPaymentMethodByName getPaymentMethodByName,
            IGetTransactions getTransactions)
        {
            this.RuleFor(model => model)
                .MustAsync((transaction, _) => new Filter<bool>(async filters =>
                    {
                        var entities = await getTransactions.GetResult(NewGetByAllParam(
                            email,
                            filters));

                        return entities.Match(
                            _ => false,
                            page => !page.Data.Any());
                    })
                    .Where("year".Equal(transaction.Payment.Date.Year)
                        .And("month".Equal(transaction.Payment.Date.Month))
                        .And("day".Equal(transaction.Payment.Date.Day))
                        .And("value".Equal(transaction.Payment.Value))
                        .And("description".Equal(transaction.Description)))
                    .Build())
                .WithMessage("Transaction already exists.");

            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Type is required.");

            this.RuleFor(model => model.Payment)
                .NotNull()
                .WithMessage("Payment is required.")
                .SetValidator(new PaymentModelValidator(email, getCurrencyByName, getPaymentMethodByName));

            this.RuleFor(model => model.Payee)
                .NotEmpty()
                .WithMessage("Payee is required.")
                .MustAsync(async (payee, _) => await getPayeeByName.GetResult(NewGetByIdParam(email, payee)))
                .WithMessage("Payee not found.");

            this.RuleFor(model => model.Category)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MustAsync(async (category, _) => await getCategoryByName.GetResult(NewGetByIdParam(email, category)))
                .WithMessage("Category not found.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must be at least one tag for transaction.");
        }
    }
}
