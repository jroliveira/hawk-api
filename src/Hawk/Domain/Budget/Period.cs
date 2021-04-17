namespace Hawk.Domain.Budget
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Shared.Money;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.Convert;

    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Domain.Shared.Money.Money;
    using static Hawk.Domain.Shared.Transaction.TransactionType;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Period
    {
        private readonly Option<int> monthOption;
        private readonly Option<int> yearOption;
        private readonly Money limit;
        private readonly IReadOnlyCollection<Transaction> transactions;

        private Period(
            in Option<int> monthOption,
            in Option<int> yearOption,
            in Money limit,
            in IEnumerable<Transaction> transactions)
        {
            this.monthOption = monthOption;
            this.yearOption = yearOption;
            this.limit = limit;
            this.transactions = transactions.ToList();
        }

        public Option<DateTime> FromOption => this.transactions.Any()
            ? Some(this.transactions.Min(transaction => transaction.Date))
            : Some(new DateTime(this.yearOption.GetOrElse(1970), this.monthOption.GetOrElse(1), 1));

        public Option<DateTime> ToOption => this.transactions.Any()
            ? Some(this.transactions.Max(transaction => transaction.Date))
            : Some(new DateTime(
                this.yearOption.GetOrElse(1970),
                this.monthOption.GetOrElse(12),
                new DateTime(this.yearOption.GetOrElse(1970), this.monthOption.GetOrElse(12), 1).AddMonths(1).AddDays(-1).Day));

        public Try<Money> Amount => NewMoney(
            this.transactions.Aggregate(this.limit.Value, (total, next) => next.Type switch
            {
                Income => total + next.Cost.Value,
                _ => total - next.Cost.Value,
            }),
            NewCurrency("EUR", "€"));

        public uint Transactions => ToUInt32(this.transactions.Count);

        public static Try<Period> NewPeriod(
            in Option<int> monthOption,
            in Option<int> yearOption,
            in Option<Money> limitOption,
            in Option<IEnumerable<Option<Transaction>>> transactionsOption) =>
                limitOption
                && transactionsOption
                    ? new Period(
                        monthOption,
                        yearOption,
                        limitOption.Get(),
                        transactionsOption.GetOrElse(new List<Option<Transaction>>()).Select(transaction => transaction.Get()))
                    : Failure<Period>(new InvalidObjectException("Invalid period."));
    }
}
