namespace Hawk.WebApi.Features.Transaction
{
    using System.Linq;
    using System.Text;

    using FluentValidation;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction.Queries;

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
                .MustAsync(async (transaction, _) =>
                {
                    var filters = new StringBuilder();
                    filters.Append($"&filter[where][year]={transaction.Payment.Date.Year}");
                    filters.Append($"&filter[where][month]={transaction.Payment.Date.Month}");
                    filters.Append($"&filter[where][day]={transaction.Payment.Date.Day}");
                    filters.Append($"&filter[where][value]={transaction.Payment.Value}");
                    filters.Append($"&filter[where][description]={transaction.Description}");

                    var entities = await getTransactions.GetResult(NewGetByAllParam(
                        email,
                        filters.ToString()));

                    return entities.Match(
                        _ => false,
                        page => !page.Data.Any());
                })
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
