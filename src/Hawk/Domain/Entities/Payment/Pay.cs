namespace Hawk.Domain.Entities.Payment
{
    using System;

    public sealed class Pay
    {
        public Pay(Price price, DateTime date, Method method)
        {
            this.Price = price;
            this.Date = date;
            this.Method = method;
        }

        public Price Price { get; }

        public DateTime Date { get; }

        public Method Method { get; }
    }
}
