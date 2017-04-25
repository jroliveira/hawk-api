namespace Finance.Entities.Transaction
{
    using System;

    public sealed class Debit : Transaction
    {
        public Debit(decimal value, DateTime date, Account account)
            : base(value, date, account)
        {
        }

        public Debit(Payment.Payment payment, Account account)
            : base(payment, account)
        {
        }
    }
}