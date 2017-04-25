namespace Finance.Entities.Transaction.Payment
{
    using System;

    public class Payment
    {
        public Payment(decimal value, DateTime date)
        {
            this.Value = value;
            this.Date = date;
        }

        public decimal Value { get; }

        public DateTime Date { get; private set; }

        public Method Method { get; set; }

        public virtual void UpdateDate(DateTime date)
        {
            this.Date = date;
        }
    }
}
