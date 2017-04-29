﻿namespace Finance.Entities.Transaction
{
    using System;

    public sealed class Debit : Transaction
    {
        public Debit(double value, DateTime date, Account account)
            : this(new Payment.Payment(value, date), account)
        {
        }

        public Debit(Payment.Payment payment, Account account)
            : this(default(int), payment, account)
        {
        }

        public Debit(int id, Payment.Payment payment, Account account)
            : base(id, payment, account)
        {
        }
    }
}