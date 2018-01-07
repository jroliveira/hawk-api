namespace Hawk.Domain.Entities.Transactions
{
    using System;

    using Hawk.Domain.Entities.Payment;

    public sealed class Credit : Transaction
    {
        public Credit(Pay pay, Account account)
            : this(Guid.NewGuid(), pay, account)
        {
        }

        public Credit(Guid id, Pay pay, Account account)
            : base(id, pay, account)
        {
        }
    }
}