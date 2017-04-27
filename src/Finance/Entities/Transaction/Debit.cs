namespace Finance.Entities.Transaction
{
    using System;

    public sealed class Debit : Transaction
    {
        public Debit(decimal value, DateTime date, Account account)
            : base(value, date, account)
        {
        }

        public Debit(int id, decimal value, DateTime date, Account account)
            : base(id, value, date, account)
        {
        }
    }
}