namespace Hawk.Domain.Budget.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static System.Int32;
    using static System.StringComparison;

    using static Hawk.Domain.Budget.Data.Neo4J.TransactionMapping;
    using static Hawk.Domain.Budget.Period;
    using static Hawk.Domain.Shared.Money.Data.Neo4J.MoneyMapping;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PeriodMapping
    {
        internal static Func<Option<Neo4JRecord>, Try<Period>> MapPeriod(IFilter filter) => recordOption => recordOption
            .Fold(Failure<Period>(NotFound(nameof(Period))))(record => NewPeriod(
                GetValue(filter, "month"),
                GetValue(filter, "year"),
                MapMoney(record.GetRecord("limit")),
                Some(record.GetListOfNeo4JRecord("transactions")
                    .Select(payee => MapTransaction(payee).ToOption()))));

        private static Option<int> GetValue(IFilter filter, string field)
        {
            var value = filter.Where
                ?.FirstOrDefault(item => string.Equals(item.Field, field, CurrentCultureIgnoreCase))
                ?.Value;

            return value == default
                ? None()
                : Some(Parse(value));
        }
    }
}
