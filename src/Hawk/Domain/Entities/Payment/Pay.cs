namespace Hawk.Domain.Entities.Payment
{
    using System;

    using Hawk.Infrastructure;

    public sealed class Pay
    {
        public Pay(Price price, DateTime date, Method method)
        {
            Guard.NotNull(price, nameof(price), "Price cannot be null.");
            Guard.NotNull(method, nameof(method), "Payment method cannot be null.");

            this.Price = price;
            this.Date = date;
            this.Method = method;
        }

        public Price Price { get; }

        public DateTime Date { get; }

        public Method Method { get; }
    }
}
