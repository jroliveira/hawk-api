namespace Finance.Entities.Transaction
{
    using System;

    public sealed class Credit : Transaction
    {
        public Credit(decimal value, DateTime date, Account account)
            : base(value, date, account)
        {
        }

        public Credit(Payment.Payment payment, Account account)
            : base(payment, account)
        {
        }
    }
}