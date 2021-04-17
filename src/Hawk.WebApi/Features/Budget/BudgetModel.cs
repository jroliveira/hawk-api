namespace Hawk.WebApi.Features.Budget
{
    using Hawk.Domain.Budget;
    using Hawk.WebApi.Features.Category;
    using Hawk.WebApi.Features.Shared.Money;

    public sealed class BudgetModel
    {
        private BudgetModel(
            string id,
            string description,
            MoneyModel limit,
            RecurrenceModel recurrence,
            CategoryModel category,
            PeriodModel? period)
        {
            this.Id = id;
            this.Description = description;
            this.Limit = limit;
            this.Recurrence = recurrence;
            this.Category = category;
            this.Period = period;
        }

        public string Id { get; }

        public string Description { get; }

        public MoneyModel Limit { get; }

        public RecurrenceModel Recurrence { get; }

        public PeriodModel? Period { get; }

        public CategoryModel Category { get; }

        internal static BudgetModel NewBudgetModel(Budget entity) => new BudgetModel(
            entity.Id.ToString(),
            entity.Description,
            entity.Limit,
            entity.Recurrence,
            entity.Category,
            entity.PeriodOption);
    }
}
