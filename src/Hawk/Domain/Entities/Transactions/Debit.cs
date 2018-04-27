namespace Hawk.Domain.Entities.Transactions
{
    using System;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Monad;

    public sealed class Debit : Transaction
    {
        private Debit(Guid id, Pay pay, Account account)
            : base(id, pay, account)
        {
        }

        public static Try<Transaction> CreateWith(Option<Guid> debitIdOption, Option<Pay> payOption, Option<Account> accountOption) => CreateWith(
            debitIdOption,
            payOption,
            accountOption,
            (id, pay, account) => new Debit(id, pay, account));
    }
}