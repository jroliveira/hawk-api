namespace Hawk.Domain.Budget
{
    using System;

    using Hawk.Domain.Shared.Money;
    using Hawk.Domain.Shared.Transaction;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Transaction
    {
        private Transaction(
            in Guid id,
            in TransactionType type,
            in DateTime date,
            in Money cost)
        {
            this.Id = id;
            this.Type = type;
            this.Date = date;
            this.Cost = cost;
        }

        public Guid Id { get; }

        public TransactionType Type { get; }

        public DateTime Date { get; }

        public Money Cost { get; }

        public static Try<Transaction> NewTransaction(
            in Option<Guid> id,
            in Option<TransactionType> typeOption,
            in Option<DateTime> dateOption,
            in Option<Money> costOption) =>
                id
                && typeOption
                && dateOption
                && costOption
                    ? new Transaction(
                        id.Get(),
                        typeOption.Get(),
                        dateOption.Get(),
                        costOption.Get())
                    : Failure<Transaction>(new InvalidObjectException("Invalid transaction."));
    }
}
