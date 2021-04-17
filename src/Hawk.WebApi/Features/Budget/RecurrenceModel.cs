namespace Hawk.WebApi.Features.Budget
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Budget;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Budget.Recurrence;

    public sealed class RecurrenceModel
    {
        public RecurrenceModel(
            DateTime start,
            string frequency,
            byte interval)
        {
            this.Start = start;
            this.Frequency = frequency;
            this.Interval = interval;
        }

        [DataType(DataType.Date)]
        public DateTime Start { get; }

        [Required]
        public string Frequency { get; }

        [Required]
        public byte Interval { get; }

        public static implicit operator RecurrenceModel(Recurrence entity) => new RecurrenceModel(
            entity.Start,
            entity.Frequency.ToString(),
            entity.Interval);

        public static implicit operator Option<Recurrence>(RecurrenceModel model) => NewRecurrence(
            model.Start,
            model.Frequency.ToEnum<BudgetFrequency>(),
            model.Interval);
    }
}
