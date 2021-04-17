namespace Hawk.WebApi.Features.Budget
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Budget;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Category;
    using Hawk.WebApi.Features.Shared.Money;

    using static Hawk.Domain.Budget.Budget;

    public class CreateBudgetModel
    {
        public CreateBudgetModel(
            string description,
            MoneyModel limit,
            RecurrenceModel recurrence,
            CategoryModel category)
        {
            this.Description = description;
            this.Limit = limit;
            this.Recurrence = recurrence;
            this.Category = category;
        }

        [Required]
        public string Description { get; }

        [Required]
        public MoneyModel Limit { get; }

        [Required]
        public RecurrenceModel Recurrence { get; }

        [Required]
        public CategoryModel Category { get; }

        public static implicit operator Option<Budget>(CreateBudgetModel model) => NewBudget(
            model.Description,
            model.Limit,
            model.Recurrence,
            model.Category);
    }
}
