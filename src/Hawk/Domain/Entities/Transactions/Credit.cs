namespace Hawk.Domain.Entities.Transactions
{
    using System;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Monad;

    public sealed class Credit : Transaction
    {
        private Credit(Guid id, Pay pay, Account account)
            : base(id, pay, account)
        {
        }

        public static Try<Transaction> CreateWith(Option<Guid> creditIdOption, Option<Pay> payOption, Option<Account> accountOption) => CreateWith(
            creditIdOption,
            payOption,
            accountOption,
            (id, pay, account) => new Credit(id, pay, account));
    }
}