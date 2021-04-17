namespace Hawk.Domain.Budget.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Budget.Recurrence;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class RecurrenceMapping
    {
        internal static Try<Recurrence> MapRecurrence(Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Recurrence>(NotFound(nameof(Recurrence))))(record => NewRecurrence(
                Date(
                    record.Get<int>("year"),
                    record.Get<int>("month"),
                    record.Get<int>("day")),
                record
                    .Get<string>("frequency")
                    .Select(value => value.ToEnum<BudgetFrequency>()),
                record.Get<byte>("interval")));
    }
}
