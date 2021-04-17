namespace Hawk.WebApi.Features.Budget
{
    using System;
    using System.Linq;

    using FluentValidation;

    using Hawk.Domain.Budget.Queries;
    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;
    using Hawk.WebApi.Features.Shared.Money;

    using Http.Query.Filter.Client.Filters.Condition;

    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    using static Http.Query.Filter.Client.FilterBuilder;

    internal sealed class CreateBudgetModelValidator : AbstractValidator<CreateBudgetModel>
    {
        internal CreateBudgetModelValidator(
            Email email,
            string? id,
            IGetBudgets getBudgets,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByCode getCurrencyByCode)
        {
            this.RuleFor(model => model)
                .MustAsync(async (budget, _) =>
                {
                    if (budget?.Recurrence == null)
                    {
                        return true;
                    }

                    var entities = await getBudgets.GetResult(NewGetByAllParam(
                        email,
                        NewFilterBuilder()
                            .Where("year".Equal(budget.Recurrence.Start.Year)
                                .And("month".Equal(budget.Recurrence.Start.Month))
                                .And("day".Equal(budget.Recurrence.Start.Day))
                                .And("category".Equal(budget.Category)))
                            .Build()));

                    return entities
                        .Fold(true)(page => page.Data.All(@try => @try
                            .Fold(true)(item => id != default && item.Id.Equals(new Guid(id)))));
                })
                .WithMessage("Budget already exists.");

            this.RuleFor(model => model.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            this.RuleFor(model => model.Limit)
                .NotNull()
                .WithMessage("Limit is required.")
                .SetValidator(new MoneyModelValidator(email, getCurrencyByCode));

            this.RuleFor(model => model.Recurrence)
                .NotNull()
                .WithMessage("Recurrence is required.")
                .SetValidator(new RecurrenceModelValidator());

            this.RuleFor(model => model.Category)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MustAsync(async (category, _) => await getCategoryByName.GetResult(NewGetByIdParam(email, category.Name)))
                .WithMessage("Category not found.");
        }
    }
}
