namespace Hawk.Domain.Entities.Transactions
{
    using System;

    using Hawk.Domain.Entities.Payment;

    public sealed class Debit : Transaction
    {
        public Debit(Pay pay, Account account)
            : this(Guid.NewGuid(), pay, account)
        {
        }

        public Debit(Guid id, Pay pay, Account account)
            : base(id, pay, account)
        {
        }
    }
}