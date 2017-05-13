namespace Finance.WebApi.Models.Transaction
{
    using System;

    public class Payment
    {
        public virtual double Value { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual string Method { get; set; }

        public virtual string Currency { get; set; }
    }
}