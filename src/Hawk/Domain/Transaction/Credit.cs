namespace Hawk.Domain.Transaction
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    public sealed class Credit : Transaction
    {
        private Credit(Guid id, Payment payment, Account account)
            : base(id, payment, account)
        {
        }

        public static Try<Transaction> CreateWith(Option<Guid> creditIdOption, Option<Payment> paymentOption, Option<Account> accountOption) => CreateWith(
            creditIdOption,
            paymentOption,
            accountOption,
            (id, payment, account) => new Credit(id, payment, account));
    }
}
