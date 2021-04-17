namespace Hawk.Domain.Budget
{
    using System;

    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Shared.Money;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.Infrastructure.Uid;

    public sealed class Budget : Entity<Guid>
    {
        private Budget(
            in Guid id,
            in string description,
            in Money limit,
            in Recurrence recurrence,
            in Option<Period> period,
            in Category category)
            : base(id)
        {
            this.Description = description;
            this.Limit = limit;
            this.Recurrence = recurrence;
            this.PeriodOption = period;
            this.Category = category;
        }

        public string Description { get; }

        public Money Limit { get; }

        public Recurrence Recurrence { get; }

        public Option<Period> PeriodOption { get; }

        public Category Category { get; }

        public static Try<Budget> NewBudget(
            in Option<string> descriptionOption,
            in Option<Money> limitOption,
            in Option<Recurrence> recurrenceOption,
            in Option<Category> categoryOption) => NewBudget(
                idOption: NewGuid(),
                descriptionOption,
                limitOption,
                recurrenceOption,
                periodOption: None(),
                categoryOption);

        public static Try<Budget> NewBudget(
            in Option<Guid> idOption,
            in Option<string> descriptionOption,
            in Option<Money> limitOption,
            in Option<Recurrence> recurrenceOption,
            in Option<Period> periodOption,
            in Option<Category> categoryOption) =>
                idOption
                && descriptionOption
                && limitOption
                && recurrenceOption
                && categoryOption
                    ? new Budget(
                        idOption.Get(),
                        descriptionOption.Get(),
                        limitOption.Get(),
                        recurrenceOption.Get(),
                        periodOption,
                        categoryOption.Get())
                    : Failure<Budget>(new InvalidObjectException($"Invalid budget '{idOption.GetStringOrElse("undefined")}'."));
    }
}
