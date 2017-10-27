namespace Hawk.Entities.Transaction.Payment
{
    using System;

    public class Payment
    {
        public Payment(double value, DateTime date, Currency currency)
        {
            this.Value = value;
            this.Date = date;
            this.Currency = currency;
        }

        public double Value { get; }

        public DateTime Date { get; }

        public Currency Currency { get; }

        public Method Method { get; set; }
    }
}
