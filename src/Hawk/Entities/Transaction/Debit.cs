namespace Hawk.Entities.Transaction
{
    using System;

    public sealed class Debit : Transaction
    {
        public Debit(double value, DateTime date, string currency, Account account)
            : this(new Payment.Payment(value, date, currency), account)
        {
        }

        public Debit(Payment.Payment payment, Account account)
            : this(Guid.NewGuid(), payment, account)
        {
        }

        public Debit(Guid id, Payment.Payment payment, Account account)
            : base(id, payment, account)
        {
        }
    }
}