namespace Finance.WebApi.Models.Transaction
{
    using System;

    public class Payment
    {
        public virtual decimal Value { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual string Method { get; set; }
    }
}