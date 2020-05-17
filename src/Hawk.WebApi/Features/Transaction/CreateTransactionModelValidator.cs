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

    using Http.Query.Filter.Client.Filters.Condition;

    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    using static Http.Query.Filter.Client.FilterBuilder;

    internal sealed class CreateTransactionModelValidator : AbstractValidator<CreateTransactionModel>
    {
        internal CreateTransactionModelValidator(
            Email email,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByCode getCurrencyByCode,
            IGetPayeeByName getPayeeByName,
            IGetPaymentMethodByName getPaymentMethodByName,
            IGetTransactions getTransactions)
        {
            this.RuleFor(model => model)
                .MustAsync(async (transaction, _) =>
                {
                    if (transaction?.Payment?.Cost == null)
                    {
                        return true;
                    }

                    var entities = await getTransactions.GetResult(NewGetByAllParam(
                        email,
                        NewFilterBuilder()
                            .Where("year".Equal(transaction.Payment.Date.Year)
                                .And("month".Equal(transaction.Payment.Date.Month))
                                .And("day".Equal(transaction.Payment.Date.Day))
                                .And("value".Equal(transaction.Payment.Cost.Value))
                                .And("category".Equal(transaction.Category))
                                .And("currency".Equal(transaction.Payment.Cost.Currency.Code))
                                .And("payee".Equal(transaction.Payee))
                                .And("paymentMethod".Equal(transaction.Payment.Method)))
                            .Build()));

                    return entities.Match(
                        exception => true,
                        page => !page.Data.Any());
                })
                .WithMessage("Transaction already exists.");

            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Type is required.");

            this.RuleFor(model => model.Payment)
                .NotNull()
                .WithMessage("Payment is required.")
                .SetValidator(new PaymentModelValidator(email, getCurrencyByCode, getPaymentMethodByName));

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
