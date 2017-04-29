namespace Finance.Entities.Transaction.Payment
{
    using System;

    public class Payment
    {
        public Payment(double value, DateTime date)
        {
            this.Value = value;
            this.Date = date;
        }

        public double Value { get; }

        public DateTime Date { get; }

        public Method Method { get; set; }
    }
}
