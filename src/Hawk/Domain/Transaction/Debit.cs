namespace Hawk.Domain.Transaction
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    public sealed class Debit : Transaction
    {
        private Debit(Guid id, Payment payment, Account account)
            : base(id, payment, account)
        {
        }

        public static Try<Transaction> CreateWith(Option<Guid> debitIdOption, Option<Payment> paymentOption, Option<Account> accountOption) => CreateWith(
            debitIdOption,
            paymentOption,
            accountOption,
            (id, payment, account) => new Debit(id, payment, account));
    }
}
