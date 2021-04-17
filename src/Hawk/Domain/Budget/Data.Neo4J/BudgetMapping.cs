namespace Hawk.Domain.Budget.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Domain.Budget;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using Neo4j.Driver;

    using static Hawk.Domain.Budget.Budget;
    using static Hawk.Domain.Budget.Data.Neo4J.PeriodMapping;
    using static Hawk.Domain.Budget.Data.Neo4J.RecurrenceMapping;
    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Domain.Shared.Money.Data.Neo4J.MoneyMapping;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class BudgetMapping
    {
        internal static Func<IRecord, Try<Budget>> MapBudget(IFilter filter) => data => MapRecord(data, "data").
            Fold(Failure<Budget>(NotFound(nameof(Budget))))(record => NewBudget(
                record.Get<Guid>("id"),
                record.Get<string>("description"),
                MapMoney(record.GetRecord("limit")),
                MapRecurrence(record.GetRecord("recurrence")),
                MapPeriod(filter)(record.GetRecord("period")),
                MapCategory(record.GetRecord("category"))));
    }
}
