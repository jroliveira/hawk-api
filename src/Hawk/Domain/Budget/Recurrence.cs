namespace Hawk.Domain.Budget
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Recurrence
    {
        private Recurrence(
            in DateTime start,
            in BudgetFrequency frequency,
            in byte interval)
        {
            this.Start = start;
            this.Frequency = frequency;
            this.Interval = interval;
        }

        public DateTime Start { get; }

        public BudgetFrequency Frequency { get; }

        public byte Interval { get; }

        public static Try<Recurrence> NewRecurrence(
            in Option<DateTime> startOption,
            in Option<BudgetFrequency> frequencyOption,
            in Option<byte> intervalOption) =>
                startOption
                && frequencyOption
                && intervalOption
                    ? new Recurrence(startOption.Get(), frequencyOption.Get(), intervalOption.Get())
                    : Failure<Recurrence>(new InvalidObjectException("Invalid recurrence."));
    }
}
