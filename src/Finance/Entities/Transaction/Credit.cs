namespace Finance.Entities.Transaction
{
    using System;

    public sealed class Credit : Transaction
    {
        public Credit(double value, DateTime date, string currency, Account account)
            : this(new Payment.Payment(value, date, currency), account)
        {
        }

        public Credit(Payment.Payment payment, Account account)
            : this(default(int), payment, account)
        {
        }

        public Credit(int id, Payment.Payment payment, Account account)
            : base(id, payment, account)
        {
        }
    }
}